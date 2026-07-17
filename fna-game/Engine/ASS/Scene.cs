using Microsoft.Xna.Framework;
using Videogame.Engine.Textures;
using Videogame.Game.Actors;

namespace Videogame.Engine.ASS;

public abstract class Scene : IDisposable
{
    public Tilemap Tilemap;
    protected Actor CameraHandler;
    
    bool isDisposed = false;

    public Scene() {}
    ~Scene() => Dispose(false);

    public virtual void Initialize() 
    {
        LoadContent();        
        CameraHandler = ActorManager.ActorFindByType<Player>();
    }

    public virtual void Draw() 
    {
        Tilemap.Draw();
        ActorManager.DrawActors();
    }

    public virtual void Update(GameTime gameTime) 
    {
        Core.Camera.Update(Tilemap.RoomWidth, Tilemap.RoomHeight, gameTime);
        
        if (CameraHandler != null)
            Core.Camera.Position = CameraHandler.Position;
        else
            CameraHandler = ActorManager.ActorFindByType<Player>();
        
        ActorManager.UpdateAllActors(gameTime);
    }

    public virtual void UnloadContent() {}

    public virtual void LoadContent() {}

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }

        if (disposing)
        {
            UnloadContent();
        }

        isDisposed = true;
    }
}