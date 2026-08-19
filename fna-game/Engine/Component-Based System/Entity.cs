using Microsoft.Xna.Framework;

namespace Videogame.Engine.CBS;

public class Entity
{
    private List<IComponent> components = new List<IComponent>();
    public IEnumerable<IComponent> GetComponents() => components;

    protected void AddComponent(IComponent component) 
    {
        if (!components.Contains(component))
        {
            component.Owner = this;
            components.Add(component);
        }
    }

    public T GetComponent<T>() where T : IComponent
    {
        return components.OfType<T>().FirstOrDefault(); 
    }
    
    public void Update(GameTime gameTime)
    {
        foreach (var comp in GetComponents())
        {
            comp.Update(gameTime);
        }   
    }

    public void Draw()
    {
        foreach (var comp in components)
        {
            comp.Draw();
        }
    }
}