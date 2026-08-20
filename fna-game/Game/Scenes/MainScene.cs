using Videogame.Engine.CBS;
using Videogame.Entities;
using Microsoft.Xna.Framework;
using Videogame.Engine.Textures;

namespace Videogame.Scenes;

public class MainScene : Scene
{    
    private Player player;
    private Enemy enemy;
    private Tilemap tilemap;

    public override void Init()
    {
        base.Init();
        tilemap = new Tilemap("testLevel", "testTileSet", "collision", "entities");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Core.Input.Keyboard.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Z))
        {
            foreach (var e in Entities)
            {
                System.Console.WriteLine(e);
            }
        }

        //Camera.Position = player.GetComponent<TransformComponent>().Position;
    }

    public override void Draw()
    {
        base.Draw();
        tilemap.Draw();
    }

    public override void Unload()
    {
        base.Unload();
        tilemap.Unload();
    }
}