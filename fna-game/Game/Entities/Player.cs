using Videogame.Engine.CBS;
using Videogame.Engine.Input;
using Videogame.Engine.Textures;
using Microsoft.Xna.Framework;

namespace Videogame.Entities;

public class Player : Entity
{
    public Player()
    {
        var transform = new TransformComponent();
        transform.Position = Vector2.Zero;
        var spriteOptions = new SpriteOptions
        {
            Position = transform.Position,
            Origin = new Vector2(16, 16),
        };
        var sprite = new SpriteComponent("yellow", spriteOptions);
        //var anim = new AnimationComponent(SpriteOptions.Default(transform.Position), TimeSpan.FromMilliseconds(300), "red", "yellow", "purple");
        var collider = new ColliderComponent((int)transform.Position.X, (int)transform.Position.Y, (int)sprite.Width - 12, (int)sprite.Height - 12, new Vector2(6, 6))
        {
            CanCollide = true,
        };
        var physics = new PhysicsComponent();
        var controls = new ControlsComponent()
        {
            MoveSpeed = 100f,
        };
        

        AddComponent(sprite);
        //AddComponent(anim);
        AddComponent(transform);
        AddComponent(collider);
        AddComponent(physics);
        AddComponent(controls);
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