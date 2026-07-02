using Microsoft.Xna.Framework;
using Videogame.Engine;
using Videogame.Engine.Textures;

namespace Videogame.Game.Actors;

public class Square : Actor
{
    private TextureAtlas atlas;

    public override void Create()
    {
        base.Create();
        Layer = CollisionLayer.Solid;
        Position = new Vector2(200, 40);
        atlas = new TextureAtlas("atlas.bin", "atlas_data.json");
        Sprite = new Sprite(atlas, "Sprite-0002", Position);
        Collider = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
    }

    public override void Step(GameTime gameTime)
    {

        base.Step(gameTime);
    }

    public override void Draw()
    {
        base.Draw();
    }
}
