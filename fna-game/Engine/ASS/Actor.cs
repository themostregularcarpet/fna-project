using Microsoft.Xna.Framework;
using Videogame.Engine.Textures;

namespace Videogame.Engine;

[Flags]
public enum CollisionLayer
{
    None = 0,
    Player = 1 << 0,   // 1
    Enemy = 1 << 1,   // 2
    Solid = 1 << 2,   // 4
    Trigger = 1 << 3,   // 8
}

public abstract class Actor
{
    public Vector2 Position;
    public Vector2 Velocity;
    private Vector2 remainder;

    public string? Name;
    public bool IsDestroyed = false;
    public bool IsPersistent = false; // should it be destroyed on the next scene?

    public Sprite? Sprite;
    public AnimatedSprite? AnimatedSprite;

    public static List<Rectangle> TileRects = new List<Rectangle>();
    public CollisionLayer Layer = CollisionLayer.None;
    public Rectangle Collider;

    public virtual void Create() { }
    public virtual void Destroy()
    {
        IsDestroyed = true;
        Sprite?.Unload();
        AnimatedSprite?.Unload();
    }

    public virtual void Step(GameTime gameTime)
    {
        if (Sprite != null)
        {
            Collider.X = (int)(Position.X - Sprite.Origin.X);
            Collider.Y = (int)(Position.Y - Sprite.Origin.Y);
            Sprite.Position = Position;
        }
        
        if (AnimatedSprite != null)
        {
            Collider.X = (int)(Position.X - AnimatedSprite.Origin.X);
            Collider.Y = (int)(Position.Y - AnimatedSprite.Origin.Y);
            AnimatedSprite.Position = Position;
        }
        
        if (Velocity != Vector2.Zero)
            Move(Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
    }

    public virtual void Draw()
    {
        Sprite?.Draw();
        AnimatedSprite?.Draw();
    }

    public void ChangeSprite(Sprite sprite) => Sprite = sprite;
    public void ChangeAnimSprite(AnimatedSprite animSprite) => AnimatedSprite = animSprite;

    public bool Intersects(Actor other) => Collider.Intersects(other.Collider);
    public bool IntersectsX(Actor other) => other.Collider.Left < Collider.Right && Collider.Left < other.Collider.Right;
    public bool IntersectsY(Actor other) => other.Collider.Top < Collider.Bottom && Collider.Top < other.Collider.Bottom;

    public virtual void OnCollide(Actor other) { }
    
    public void Stop() => Velocity = Vector2.Zero;
    public void StopX() => Velocity.X = 0;
    public void StopY() => Velocity.Y = 0;

    public virtual void OnCollideX() => StopX();
    public virtual void OnCollideY() => StopY();

    public bool MovePixel(Point sign)
	{
		var offsetRect = new Rectangle((int)Position.X + sign.X - (int)Sprite?.Origin.X, (int)Position.Y + sign.Y - (int)Sprite?.Origin.X,
         Collider.Width, Collider.Height);

        foreach (var actor in ActorManager.Actors)
        {
            if ((actor.Layer & CollisionLayer.Solid) != 0 && actor != this)
            {
                if (offsetRect.Intersects(actor.Collider))
                {
                    return false;
                }
            }
        }

        foreach (var rect in TileRects)
        {
            if (offsetRect.Intersects(rect))
                return false;
        }

        Position.X += sign.X;
        Position.Y += sign.Y;
        return true;
	}

	public void Move(Vector2 value)
	{
		remainder += value;

        int moveX = (int)MathF.Floor(remainder.X);
        int moveY = (int)MathF.Floor(remainder.Y);
        
        remainder.X -= moveX;
        remainder.Y -= moveY;

        while (moveX != 0)
        {
            int sign = Math.Sign(moveX);
            if (!MovePixel(new Point(sign, 0)))
            {
                StopX();
                
                //          SIGNAL POBEDI!!!!!          //
                //  System.Console.WriteLine("hohol");  //
                //                                      //
                
                break;
            }
            moveX -= sign;
        }

        while (moveY != 0)
        {
            int sign = Math.Sign(moveY);
            if (!MovePixel(new Point(0, sign)))
            {
                StopY();
                break;
            }
            moveY -= sign;
        }
    }
}
