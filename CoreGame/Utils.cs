using Raylib_cs;

namespace CoreGame;

public static class Utils
{
    public static Rectangle GetSourceRect(Texture2D _texture) =>
        new Rectangle(0, 0, _texture.Width, _texture.Height);

    public static Rectangle GetSourceRect(RenderTexture2D _render_texture)
    {
        var rect = GetSourceRect(_render_texture.Texture);

        rect.Height *= -1;
        
        return rect;
    }
}