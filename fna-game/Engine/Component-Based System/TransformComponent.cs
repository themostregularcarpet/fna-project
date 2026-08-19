using Microsoft.Xna.Framework;

namespace Videogame.Engine.CBS;

public class TransformComponent : IComponent
{
    public Entity Owner { get; set; }
    public Vector2 Position = Vector2.Zero;
    public Vector2 Velocity = Vector2.Zero;
    public float Rotation = 0f;
    public float Scale = 1f;

    public void Update(GameTime gameTime) { }

    public void Draw() {}
}