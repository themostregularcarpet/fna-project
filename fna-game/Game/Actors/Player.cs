using Microsoft.Xna.Framework;
using Videogame.Engine;
using Videogame.Engine.Audio;
using Videogame.Engine.Input;
using Videogame.Engine.Textures;

namespace Videogame.Game.Actors;

public class Player : Actor
{
    private TextureAtlas atlas;
    private float pitch = 0.5f;

    public override void Create()
    {
        Name = "Player";
        Layer = CollisionLayer.Player;
        atlas = new TextureAtlas("atlas.bin", "atlas_data.json");
        Sprite = new Sprite(atlas, "Sprite-0001", Position);
        Sprite.CenterOrigin();
        Collider = new Rectangle((int)Position.X, (int)Position.Y, 20, 20);
        base.Create();
    }

    public override void Step(GameTime gameTime)
    {
        if (ActorManager.ActorCount<Player>() > 2) 
            Destroy();

        int speed = 75;
        Velocity = Vector2.Zero;
        if (Core.Input.Keyboard.IsKeyDown(Controls.Up))     Velocity.Y -= speed;
        if (Core.Input.Keyboard.IsKeyDown(Controls.Down))   Velocity.Y += speed;
        if (Core.Input.Keyboard.IsKeyDown(Controls.Left))   Velocity.X -= speed;
        if (Core.Input.Keyboard.IsKeyDown(Controls.Right))  Velocity.X += speed;

        Sprite.Rotation += 0.05f;

        if (Core.Input.Keyboard.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.T)) 
        {
            pitch -= 0.1f;
            pitch = Math.Clamp(pitch, 0f, 1f);
            System.Console.WriteLine($"pitch = {pitch}");
        }
        if (Core.Input.Keyboard.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.Y)) 
        {
            pitch += 0.1f;
            pitch = Math.Clamp(pitch, 0f, 1f);
            System.Console.WriteLine($"pitch = {pitch}");
        }

        if (Core.Input.Keyboard.WasKeyPressed(Microsoft.Xna.Framework.Input.Keys.G)) 
        {
            var instance = AudioManager.PlayEvent("event:/New Event");
            AudioManager.SetParameter(instance, "testParam", pitch);
        }

        base.Step(gameTime);
    }

    public override void Draw()
    {
        base.Draw();
    }
}