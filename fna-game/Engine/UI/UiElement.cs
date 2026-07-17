using Microsoft.Xna.Framework;

namespace Videogame.Engine.UI;

public abstract class UiElement
{
    public bool isDestroying = false;

    public virtual void Start() {}
    public virtual void Update(GameTime gameTime) {}
    public virtual void Draw() {}
    public void Destroy()
    {
        isDestroying = true;
    }
}