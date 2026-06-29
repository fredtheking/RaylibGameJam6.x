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
    
    public static void Init(GameMode _mode)
    {
        var flags = ConfigFlags.VSyncHint;

        if (_mode == GameMode.Desktop)
            flags |= ConfigFlags.AlwaysRunWindow | ConfigFlags.ResizableWindow;
        
        Raylib.SetConfigFlags(flags);
        Raylib.InitWindow
            ((int)GameState.TargetSize.X, 
            (int)GameState.TargetSize.Y, 
            $"Raylib GameJam ({_mode.ToString()} Version)"
        );
        Raylib.InitAudioDevice();

        State = new GameState(_mode);
    }

    public static void DesktopDeinit()
    {
        MusicPlayer.Unload();
        
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
        var mouseX = (screenMouse.X - destX) * (GameState.TargetSize.X / renderSize);
        var mouseY = (screenMouse.Y - destY) * (GameState.TargetSize.Y / renderSize);
        
        ref var mouse = ref State.VirtualMouse;
        
        mouse.X = mouseX;
        mouse.Y = mouseY;
        
        mouse.X = Math.Clamp(mouse.X, 0, GameState.TargetSize.X);
        mouse.Y = Math.Clamp(mouse.Y, 0, GameState.TargetSize.Y);
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
                Raylib.ToggleBorderlessWindowed();
                InformFullscreen(Raylib.IsWindowState(ConfigFlags.BorderlessWindowMode));
            }
        }
        MusicPlayer.Process();
        FitTextureRecalc();

        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Black);


        Raylib.BeginTextureMode(State.FitTexture);
        Raylib.ClearBackground(GameState.BackgroundColor);
        Draw();
        Raylib.EndTextureMode();
        Raylib.DrawTexturePro(
            State.FitTexture.Texture, 
            Utils.GetSourceRect(State.FitTexture),
            State.FitTextureDestRect,
            Vector2.Zero, 0, Color.White
        );
        
        Raylib.EndDrawing();
    }

    private static void Draw()
    {
        Raylib.DrawTexture(State!.Logo, 584, 584, Color.White);
        Raylib.DrawText($"Time: {Raylib.GetTime()}", 20, 60, 32, Color.Blue);
        Raylib.DrawText($"Muted: {State.IsMuted}", 20, 100, 32, Color.Magenta);
        Raylib.DrawText($"Fullscreen: {State.IsFullscreen}", 20, 140, 32, Color.Purple);
        Raylib.DrawText($"Render Size: {new Vector2(Raylib.GetRenderWidth(), Raylib.GetRenderHeight()).ToString()}", 20, 180, 32, Color.Pink);
        Raylib.DrawText($"Screen Size: {new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()).ToString()}", 20, 220, 32, Color.Red);
        Raylib.DrawText($"Using framebuffer: {Raylib.IsRenderTextureValid(State.FitTexture)}", 20, 260, 32, Color.Beige);

        Raylib.DrawCircleV(State.VirtualMouse, 10, Color.Green);
        
        Raylib.DrawFPS(10, 10);
    }
}