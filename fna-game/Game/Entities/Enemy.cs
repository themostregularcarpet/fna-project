using Videogame.Engine.CBS;
using Videogame.Engine.Textures;
using Microsoft.Xna.Framework;

namespace Videogame.Entities;

public class Enemy : Entity
{
    TransformComponent tc;
    ColliderComponent coll;

    public Enemy()
    {
        var transform = new TransformComponent();
        transform.Position = new Vector2(100, 100);
        var sprite = new SpriteComponent("yellow", SpriteOptions.Default(transform.Position));
        var collider = new ColliderComponent((int)transform.Position.X, (int)transform.Position.Y, (int)sprite.Width, (int)sprite.Height, Vector2.Zero);

        collider.CanCollide = true;

        tc = transform;
        coll = collider;

        AddComponent(sprite);
        AddComponent(transform);
        AddComponent(collider);
    }

    public void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public void Draw()
    {
        base.Draw();
    }
}