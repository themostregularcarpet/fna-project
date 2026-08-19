using Microsoft.Xna.Framework;

namespace Videogame.Engine.CBS;

public interface IComponent
{
    public Entity Owner { get; set; }
    public void Update(GameTime gameTime);
    public void Draw();
}