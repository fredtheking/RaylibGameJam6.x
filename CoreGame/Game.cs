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
            flags |= ConfigFlags.AlwaysRunWindow;
        
        Raylib.SetConfigFlags(flags);
        Raylib.InitWindow(720, 720, $"Raylib GameJam ({_mode.ToString()} Version)");
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
    
    public static void Frame()
    {
        if (State is null) return;

        if (State.Mode == GameMode.Desktop)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.F1)) 
                MusicPlayer.RollNew();
            if (Raylib.IsKeyPressed(KeyboardKey.F10))
                SwitchMuted();

            if (Raylib.IsKeyPressed(KeyboardKey.F11))
            {
                Raylib.ToggleFullscreen();
                InformFullscreen(Raylib.IsWindowFullscreen());
            }
        }
        MusicPlayer.Process();
        
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.DarkGray);
        
        Raylib.DrawTexture(State.Logo, 584, 584, Color.White);
        Raylib.DrawText($"Muted: {State.IsMuted}", 20, 100, 32, Color.Magenta);
        Raylib.DrawText($"Fullscreen: {State.IsFullscreen}", 20, 140, 32, Color.Purple);

        Raylib.DrawFPS(10, 10);
        Raylib.EndDrawing();
    }
}