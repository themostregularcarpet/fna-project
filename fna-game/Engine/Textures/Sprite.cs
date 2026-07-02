using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Videogame.Engine.Textures;

public class Sprite 
{
    public Vector2 Position { get; set; }
    public Color Color { get; set; } = Color.White;
    public Vector2 Origin { get; set; } = Vector2.Zero;
    public Vector2 Scale { get; set; } = Vector2.One;
    public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
    public float LayerDepth { get; set; } = 0.0f;
    public float Rotation { get; set; } = 0.0f;
    public int Width;
    public int Height;

    private TextureAtlas atlas;
    private string name;
    
    public Sprite() {}

    public Sprite(TextureAtlas atlas, string name, Vector2 pos)
    {
        this.atlas = atlas;
        this.name = name;
        Position = pos;
        var rect = atlas.GetRect(name);
        Width = rect.Width;
        Height = rect.Height;
    }

    public void CenterOrigin() => Origin = new Vector2(Width * Scale.X / 2, Height * Scale.Y / 2);

    public void Draw()
    {
        var rect = atlas.GetRect(name);
        Core.SpriteBatch.Draw(atlas.GetAtlas(), Position, rect, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
    }

    public void Unload()
    {
        atlas = null;
        name = null;
    }
}