using System.Numerics;
using Raylib_cs;

namespace CoreGame;

public class GameState : IDisposable
{
    public Texture2D Logo = default;
    public GameMode Mode;
    public bool IsMuted = false;
    public bool IsFullscreen = false;

    public static readonly Vector2 StartSize = new Vector2(720, 720); 
    public static readonly Color BackgroundColor = Color.DarkGray;

    public Rectangle FitTextureDestRect = default;
    public Vector2 VirtualMouse = default;
    public Vector2 LastWindowSize = default;
    
    public bool IsDesktop => Mode is GameMode.Desktop;
    public bool IsWASM => Mode is GameMode.WASM;

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