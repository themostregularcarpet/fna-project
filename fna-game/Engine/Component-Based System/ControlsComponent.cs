using Videogame.Engine.Input;
using Microsoft.Xna.Framework;

namespace Videogame.Engine.CBS;

public class ControlsComponent : IComponent
{
    public Entity Owner { get; set; }
    public float MoveSpeed = 0f;
    public bool NormalizeVector = true;
    
    public void Update(GameTime gameTime)
    {
        var transform = Owner.GetComponent<TransformComponent>();

        if (transform != null)
        {
            var direction = Vector2.Zero;

            if (Core.Input.Keyboard.IsKeyDown(Controls.Left))
            {
                direction.X -= 1;
            }
            if (Core.Input.Keyboard.IsKeyDown(Controls.Right))
            {
                direction.X += 1;
            }
            if (Core.Input.Keyboard.IsKeyDown(Controls.Up))
            {
                direction.Y -= 1;
            }
            if (Core.Input.Keyboard.IsKeyDown(Controls.Down))
            {
                direction.Y += 1;
            }

            if (direction != Vector2.Zero && NormalizeVector)
            {
                direction.Normalize();
            }

            transform.Velocity = direction * MoveSpeed;
        }
    }

    public void Draw() {}
}