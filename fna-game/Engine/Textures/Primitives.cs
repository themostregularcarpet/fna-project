using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Videogame.Engine.Textures;

public static class Primitives
{
    public static void DrawRectangle(Rectangle rect, Color color)
    {
        Texture2D rectTexture = new Texture2D(Core.GraphicsDevice, 1, 1);
        rectTexture.SetData(new[] {color});
        Core.SpriteBatch.Draw(rectTexture, rect, color);
    }
}