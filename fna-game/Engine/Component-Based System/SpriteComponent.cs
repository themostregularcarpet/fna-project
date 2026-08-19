using Videogame.Engine.Textures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Videogame.Engine.CBS;

public class SpriteComponent : IComponent
{
    public Entity Owner { get; set; }
    public SpriteOptions SpriteOptions;
    public Rectangle SpriteRect;
    public float Width => SpriteRect.Width * SpriteOptions.Scale;
    public float Height => SpriteRect.Height * SpriteOptions.Scale;
    public bool IsVisible = true;
    private string spriteName;

    public SpriteComponent(string spriteName, SpriteOptions so)
    {
        this.spriteName = spriteName;
        var rect = Core.Atlas.GetSpriteRect(spriteName);
        SpriteRect = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        SpriteOptions = so;
    }

    public void Update(GameTime gameTime)
    {
        var transform = Owner.GetComponent<TransformComponent>();
        if (transform != null)
        {
            SpriteOptions.Position = transform.Position;
            SpriteOptions.Rotation = transform.Rotation;
            SpriteOptions.Scale = transform.Scale;
        }
    }

    public void Draw()
    {
        if (IsVisible)
        {
            Core.Atlas.DrawSprite(spriteName, SpriteOptions);
        }
    }

    public void ChangeSprite(string name) 
    {
        spriteName = name;
        SpriteRect = Core.Atlas.GetSpriteRect(name);
    }
}