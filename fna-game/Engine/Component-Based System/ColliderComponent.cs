using Microsoft.Xna.Framework;

namespace Videogame.Engine.CBS;

public class ColliderComponent : IComponent
{
    public Entity Owner { get; set; }
    public Rectangle Collider;
    public Vector2 Offset = Vector2.Zero;
    public bool CanCollide = true;

    public ColliderComponent(int x, int y, int width, int height, Vector2 offset)
    {
        Collider = new Rectangle(x, y, width, height);
        Offset = offset;
    }
    
    public void Update(GameTime gameTime) { }

    public void Draw() {}
}