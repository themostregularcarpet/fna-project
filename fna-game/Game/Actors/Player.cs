using Microsoft.Xna.Framework;
using Videogame.Engine;
using Videogame.Engine.Input;
using Videogame.Engine.Textures;

namespace Videogame.Game.Actors;

public class Player : Actor
{
    private TextureAtlas atlas;

    public override void Create()
    {
        Name = "Player";
        Layer = CollisionLayer.Player;
        atlas = new TextureAtlas("atlas.bin", "atlas_data.json");
        Sprite = new Sprite(atlas, "Sprite-0001", Position);
        Sprite.CenterOrigin();
        Collider = new Rectangle((int)Position.X - 40, (int)Position.Y - 50, 20, 20);
        base.Create();
    }

    public override void Step(GameTime gameTime)
    {
        if (ActorManager.ActorCount<Player>() > 2) 
            Destroy();

        float speed = 75f;
        Velocity = Vector2.Zero;

        if (Core.Input.Keyboard.IsKeyDown(Controls.Up))     Velocity.Y -= speed;
        if (Core.Input.Keyboard.IsKeyDown(Controls.Down))   Velocity.Y += speed;
        if (Core.Input.Keyboard.IsKeyDown(Controls.Left))   Velocity.X -= speed;
        if (Core.Input.Keyboard.IsKeyDown(Controls.Right))  Velocity.X += speed;

        Sprite.Rotation += 0.05f;

        base.Step(gameTime);
    }

    public override void Draw()
    {
        base.Draw();
    }
}