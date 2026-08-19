using MainGame.Scenes;
using Microsoft.Xna.Framework;

namespace Videogame;

class MainGame : Core
{
    [STAThread]
    static void Main(string[] args)
    {
        using (MainGame g = new MainGame())
        {
            g.Run();
        }
    }

    public MainGame() : base(1280, 720, "game", false, true, false)
    {
    }

    protected override void Initialize()
    {
        base.Initialize();
        ChangeScene(new MainScene());
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}