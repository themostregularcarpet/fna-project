using Videogame.Engine.CBS;
using Videogame.Entities;
using Microsoft.Xna.Framework;
using Videogame.Engine.Textures;

namespace Videogame.Scenes;

public class MainScene : Scene
{    
    private Enemy enemy;

    public override void Init()
    {
        base.Init();
        Tilemap = new Tilemap("testLevel", "testTileSet", "collision", "entities");
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Z))
        {
            foreach (var e in Entities)
            {
                System.Console.WriteLine(e);
            }
        }

        var playerPos = GetEntity<Player>().GetComponent<TransformComponent>();
        Camera.Position = playerPos.Position;
    
        base.Update(gameTime);
    }

    public override void Draw()
    {
        base.Draw();
    }

    public override void Unload()
    {
        base.Unload();
    }
}