using Microsoft.Xna.Framework;

namespace Videogame.Engine.CBS;

public abstract class Scene
{
    public static List<Entity> Entities = new();
    private List<Entity> entitiesToAdd = new();
    private List<Entity> entitiesToRemove = new();

    private int centerCamX = 160;
    private int centerCamY = 90;

    public CameraComponent Camera { get; private set; }

    public void AddEntity(Entity entity) => entitiesToAdd.Add(entity);

    public static Entity AddEntity(string name)
    {
        string className = char.ToUpper(name[0]) + name.Substring(1);
        string fullName = $"Videogame.Entities.{className}";
        
        Type type = Type.GetType(fullName, true);
        var entity = (Entity)Activator.CreateInstance(type);
        Entities.Add(entity);
        return entity;
    }

    public void RemoveEntity(Entity entity) => entitiesToRemove.Add(entity);

    public virtual void Init()
    {
        Camera = new CameraComponent(new Vector2(centerCamX, centerCamY));
        Camera.Zoom = 1f;
    }

    public virtual void Update(GameTime gameTime)
    {
        Camera?.Update(centerCamX, centerCamY, gameTime);
        foreach (var entity in entitiesToAdd)
        {
            Entities.Add(entity);
        }
        entitiesToAdd.Clear();

        foreach (var entity in entitiesToRemove)
        {
            Entities.Remove(entity);
        }
        entitiesToRemove.Clear();

        foreach (var entity in Entities)
        {
            entity.Update(gameTime);
        }
    }

    public virtual void Draw()
    {
        //foreach (var entity in Entities)
        //{
        //    entity.Draw();
        //}
    }

    public virtual void Unload()
    {
        Entities.Clear();
        entitiesToAdd.Clear();
        entitiesToRemove.Clear();
    }
}