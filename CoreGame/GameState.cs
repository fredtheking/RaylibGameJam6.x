using System.Numerics;
using Raylib_cs;

namespace CoreGame;

public class GameState : IDisposable
{
    public Texture2D Logo = default;
    public GameMode Mode;
    public bool IsMuted = false;
    public bool IsFullscreen = false;

    public static readonly Vector2 TargetSize = new Vector2(720, 720); 
    public static readonly Color BackgroundColor = Color.DarkGray;

    public RenderTexture2D FitTexture = default;
    public Rectangle FitTextureDestRect = default;
    public Vector2 VirtualMouse = default;
    
    public bool IsDesktop => Mode is GameMode.Desktop;
    public bool IsWASM => Mode is GameMode.WASM;

    public GameState(GameMode _mode)
    {
        Mode = _mode;
        
        Logo = Raylib.LoadTexture("Resources/raylib_logo.png");
        
        FitTexture = Raylib.LoadRenderTexture((int)TargetSize.X, (int)TargetSize.Y);
    }

    public void Dispose()
    {
        Raylib.UnloadRenderTexture(FitTexture);
        
        Raylib.UnloadTexture(Logo);
    }
}