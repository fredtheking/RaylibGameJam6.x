using System.Numerics;
using Raylib_cs;

namespace CoreGame;

public struct RenderPack
{
    public Texture2D Texture;
    public Rectangle Source;
    
    public RenderPack(Texture2D _texture, Rectangle? _source = null)
    {
        Texture = _texture;
        Source = _source ?? new Rectangle(Vector2.Zero, Texture.Dimensions);
    }

    public RenderPack(RenderTexture2D _render_texture)
        : this(_render_texture.Texture, new Rectangle(
            0, 0, _render_texture.Texture.Width, -_render_texture.Texture.Height))
    {}  
}