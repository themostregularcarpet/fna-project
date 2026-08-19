using Videogame.Engine.CBS;
using Engine.Entities;
using Microsoft.Xna.Framework;

namespace MainGame.Scenes;

public class MainScene : Scene
{    
    private Player player;
    private Enemy enemy;


    public override void Init()
    {
        base.Init();
        player = new Player();
        AddEntity(player);
        enemy = new Enemy();
        AddEntity(enemy);
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