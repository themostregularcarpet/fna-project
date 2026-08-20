using Microsoft.Xna.Framework;
using Videogame.Engine.Textures;

namespace Videogame.Engine.CBS;

public abstract class Scene
{
    public static List<Entity> Entities = new();
    private List<Entity> entitiesToAdd = new();
    private List<Entity> entitiesToRemove = new();

    public CameraComponent Camera { get; private set; }
    public Tilemap? Tilemap { get; set; }

    public void AddEntity(Entity entity) => entitiesToAdd.Add(entity);

    public static Entity AddEntity(string name)
    {
        var entity = EntityRegistry.Create(name);
        Entities.Add(entity);
        return entity;
    }

    public void RemoveEntity(Entity entity) => entitiesToRemove.Add(entity);

    public Entity GetEntity<T>() where T : Entity => Entities.OfType<T>().FirstOrDefault();

    public virtual void Init()
    {
        Camera = new CameraComponent(new Vector2(160, 90));
        Camera.Zoom = 1f;
        System.Console.WriteLine("cam initialized!");
    }

    public virtual void Update(GameTime gameTime)
    {
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

        if (Tilemap != null)
        {
            Camera.Update(Tilemap.RoomWidth, Tilemap.RoomHeight, gameTime);
        } 
        else
        {
            Camera.Update(320, 180, gameTime);
        }
    }

    public virtual void Draw()
    {
        //foreach (var entity in Entities)
        //{
        //    entity.Draw();
        //}
        Tilemap?.Draw();
    }

    public virtual void Unload()
    {
        Entities.Clear();
        entitiesToAdd.Clear();
        entitiesToRemove.Clear();
        Tilemap?.Unload();
    }
}