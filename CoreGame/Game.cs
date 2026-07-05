using System.Numerics;
using Raylib_cs;

namespace CoreGame;

public enum GameMode
{
    Desktop,
    WASM,
}

public class Game
{
    public static GameState? State = null;
    public static Canvas? Canvas = null;
    
    public static void Init(GameMode _mode)
    {
        var flags = ConfigFlags.VSyncHint;

        if (_mode == GameMode.Desktop)
            flags |= ConfigFlags.AlwaysRunWindow | ConfigFlags.ResizableWindow;
        
        Raylib.SetConfigFlags(flags);
        Raylib.InitWindow
            ((int)GameState.StartSize.X, 
            (int)GameState.StartSize.Y, 
            $"Raylib GameJam ({_mode.ToString()} Version)"
        );
        Raylib.InitAudioDevice();

        State = new GameState(_mode);
        Canvas = new Canvas();
    }

    public static void DesktopDeinit()
    {
        MusicPlayer.Unload();
        
        Canvas?.Dispose();
        Canvas = null;
        
        State?.Dispose();
        State = null;
        
        Raylib.CloseAudioDevice();
        Raylib.CloseWindow();
    }

    public static bool ShouldRun =>
        !Raylib.WindowShouldClose();
    public static bool SwitchMuted()
    {
        if (State is null) return false;
        
        State.IsMuted = !State.IsMuted;
        Raylib.SetMasterVolume(State.IsMuted ? 0.0f : 1.0f);
        
        return State.IsMuted;
    }
    public static void InformFullscreen(bool _is_fullscreen) =>
        State?.IsFullscreen = _is_fullscreen;
    public static void InformFullscreen(bool _is_fullscreen, int _screen_width, int _screen_height)
    {
        InformFullscreen(_is_fullscreen);
        Raylib.SetWindowSize(_screen_width, _screen_height);
    }

    private static void FitTextureRecalc()
    {
        float screenX = Raylib.GetScreenWidth();
        float screenY = Raylib.GetScreenHeight();
        
        var renderSize = Math.Min(screenX, screenY);

        var destX = (screenX - renderSize) / 2.0f;
        var destY = (screenY - renderSize) / 2.0f;

        ref var rect = ref State!.FitTextureDestRect;

        rect.X = destX;
        rect.Y = destY;
        rect.Width = rect.Height = renderSize;

        
        var screenMouse = Raylib.GetMousePosition();
        var mouseX = (screenMouse.X - destX) * (GameState.StartSize.X / renderSize);
        var mouseY = (screenMouse.Y - destY) * (GameState.StartSize.Y / renderSize);
        
        ref var mouse = ref State.VirtualMouse;
        
        mouse.X = mouseX;
        mouse.Y = mouseY;
        
        mouse.X = Math.Clamp(mouse.X, 0, GameState.StartSize.X);
        mouse.Y = Math.Clamp(mouse.Y, 0, GameState.StartSize.Y);
    }
    
    public static void Frame()
    {
        if (State is null) return;

        if (State.IsDesktop)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.F1)) 
                MusicPlayer.RollNew();
            if (Raylib.IsKeyPressed(KeyboardKey.F10))
                SwitchMuted();

            if (Raylib.IsKeyPressed(KeyboardKey.F11))
            {
                var wasFullscreen = Raylib.IsWindowFullscreen();

                if (wasFullscreen)
                { // going window
                    Raylib.SetWindowSize(
                        (int)State.LastWindowSize.X,
                        (int)State.LastWindowSize.Y
                    );

                    State.LastWindowSize = default;
                }
                else
                { // going fullscreen
                    State.LastWindowSize.X = Raylib.GetRenderWidth();
                    State.LastWindowSize.Y = Raylib.GetRenderHeight();
                    
                    var monitor = Raylib.GetCurrentMonitor();
                    Raylib.SetWindowSize(
                        Raylib.GetMonitorWidth(monitor),
                        Raylib.GetMonitorHeight(monitor)
                    );
                }
                
                Raylib.ToggleFullscreen();
                InformFullscreen(Raylib.IsWindowFullscreen());
            }
        }
        MusicPlayer.Process();
        FitTextureRecalc();

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);
        
        var rp = Canvas!.Render();
        Raylib.DrawTexturePro(
            rp.Texture, 
            rp.Source,
            State.FitTextureDestRect,
            Vector2.Zero, 0, Color.White
        );
        
        Raylib.EndDrawing();
    }
}