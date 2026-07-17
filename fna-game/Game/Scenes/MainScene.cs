using Microsoft.Xna.Framework;
using Videogame.Engine;
using Videogame.Game.Actors;
using Videogame.Engine.ASS;
using Videogame.Engine.Textures;
using FontStashSharp;

namespace Videogame.Game.Scenes;

public class MainScene : Scene
{
    public override void Initialize()
    {
        base.Initialize();
        ActorManager.CreateActor<Square>();
        Tilemap = new Tilemap("testLevel", "testTileSet", "collision", "entities");

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw()
    {
        base.Draw();
    }
}