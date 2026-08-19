using Microsoft.Xna.Framework;

namespace Videogame.Engine.CBS;

public class PhysicsComponent : IComponent
{
    public Entity Owner { get; set; }

    private Vector2 remainder;

    public void Update(GameTime gameTime)
    {
        var transform = Owner.GetComponent<TransformComponent>();
        if (transform != null)
        {
            if (transform.Velocity != Vector2.Zero)
            {
                Move(transform, transform.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }            
        }
    }

    private bool MovePixel(TransformComponent transform, Point sign)
    {
        var collider = Owner.GetComponent<ColliderComponent>();
        if (collider == null)
        {
            transform.Position.X += sign.X;
            transform.Position.Y += sign.Y;
            return true;
        }

        var sprite = Owner.GetComponent<SpriteComponent>();
        int offsetX = sprite != null ? (int)sprite.SpriteOptions.Origin.X : 0;
        int offsetY = sprite != null ? (int)sprite.SpriteOptions.Origin.Y : 0;
        var collisionPredictRect = new Rectangle((int)(transform.Position.X + sign.X - offsetX + collider.Offset.X),
                                                 (int)(transform.Position.Y + sign.Y - offsetY + collider.Offset.Y),
                                                 collider.Collider.Width, collider.Collider.Height);

        foreach (var entity in Scene.Entities)
        {
            var entityCollider = entity.GetComponent<ColliderComponent>();
            if (entityCollider != null)
            {
                if (collisionPredictRect.Intersects(entityCollider.Collider) && entityCollider.CanCollide) 
                {
                    if (entity != Owner)
                    {
                        return false;
                    }
                }
            }
        }


        transform.Position.X += sign.X;
        transform.Position.Y += sign.Y;
        return true;
    }

    private void Move(TransformComponent transform, Vector2 value)
    {
        remainder += value;

        int moveX = (int)MathF.Floor(remainder.X);
        int moveY = (int)MathF.Floor(remainder.Y);

        remainder.X -= moveX;
        remainder.Y -= moveY;

        while (moveX != 0)
        {
            int sign = Math.Sign(moveX);

            if (!MovePixel(transform, new Point(sign, 0)))
            {
                transform.Velocity.X = 0f;
                remainder.X = 0;
                break;
            }
            moveX -= sign;
        }

        while (moveY != 0)
        {
            int sign = Math.Sign(moveY);

            if (!MovePixel(transform, new Point(0, sign)))
            {
                transform.Velocity.Y = 0f;
                remainder.Y = 0;
                break;
            }
            moveY -= sign;
        }
    }
    
    public void Draw() {}
}