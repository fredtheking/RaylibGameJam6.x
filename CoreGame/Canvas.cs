using System.Numerics;
using CoreGame.Interfaces;
using Raylib_cs;

namespace CoreGame;

public class Canvas : IRenderable, IDisposable
{
    public RenderTexture2D RT;
    public Rectangle RTDest = default;
    
    public Canvas()
    {
        RT = Raylib.LoadRenderTexture(
            (int)GameState.StartSize.X,
            (int)GameState.StartSize.Y
        );
    }

    public void Dispose()
    {
        Raylib.UnloadRenderTexture(RT);
    }

    public void Dispatch()
    {
        
    }

    public RenderPack Render()
    {
        Raylib.BeginTextureMode(RT);
        Raylib.ClearBackground(GameState.BackgroundColor);
        
        Raylib.DrawTexture(Game.State!.Logo, 584, 584, Color.White);
        Raylib.DrawText($"Time: {Raylib.GetTime()}", 20, 60, 32, Color.Blue);
        Raylib.DrawText($"Muted: {Game.State.IsMuted}", 20, 100, 32, Color.Magenta);
        Raylib.DrawText($"Fullscreen: {Game.State.IsFullscreen}", 20, 140, 32, Color.Purple);

        Raylib.DrawCircleV(Game.State.VirtualMouse, 10, Color.Green);
        
        Raylib.DrawFPS(10, 10);
        
        Raylib.EndTextureMode();
        
        return new RenderPack(RT);
    }
}