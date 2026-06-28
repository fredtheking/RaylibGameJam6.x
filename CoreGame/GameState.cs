using Raylib_cs;

namespace CoreGame;

public class GameState : IDisposable
{
    public Texture2D Logo = default;
    public GameMode Mode;
    public bool IsMuted = false;
    public bool IsFullscreen = false;

    public GameState(GameMode _mode)
    {
        Mode = _mode;
        
        Logo = Raylib.LoadTexture("Resources/raylib_logo.png");
    }

    public void Dispose()
    {
        Raylib.UnloadTexture(Logo);
    }
}