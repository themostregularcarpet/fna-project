using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Videogame.Engine.Textures;

public struct SpriteOptions
{
    public Vector2 Position = Vector2.Zero;
    public Color Color = Color.White;
    public float Rotation = 0f;
    public Vector2 Origin = Vector2.Zero;
    public float Scale = 1f;
    public SpriteEffects SpriteEffects = SpriteEffects.None;
    public float LayerDepth = 0f;

    public SpriteOptions()
    {
    }

    public static SpriteOptions Default(Vector2 position) => new SpriteOptions 
    {
        Position = position,
        Color = Color.White,
        Rotation = 0f,
        Origin = Vector2.Zero,
        Scale = 1f,
        SpriteEffects = SpriteEffects.None,
        LayerDepth = 0f,
    };
}