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

    public RenderPack Render()
    {
        Raylib.BeginTextureMode(RT);
        Raylib.ClearBackground(GameState.BackgroundColor);
        
        Raylib.DrawTexture(Game.State!.Logo, 584, 584, Color.White);
        Raylib.DrawCircleV(Game.State.VirtualMouse, 10, Color.Green);
        
        Raylib.EndTextureMode();
        
        return new RenderPack(RT);
    }
}