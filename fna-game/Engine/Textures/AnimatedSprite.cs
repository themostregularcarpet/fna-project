using Microsoft.Xna.Framework;

namespace Videogame.Engine.Textures;

public class AnimatedSprite : Sprite
{
    public TimeSpan Delay;
    public bool IsAnimating = true;
    private int currFrame;
    private TimeSpan time;
    private List<string> sprites = new List<string>();
    private TextureAtlas atlas;

    public AnimatedSprite() {}

    public AnimatedSprite(TimeSpan delay, TextureAtlas atlas, params string[] sprites) 
    {
        this.atlas = atlas;
        Delay = delay;
        this.sprites.AddRange(sprites);
    }

    public void Update(GameTime gameTime)
    {
        if (!IsAnimating || sprites.Count == 0) return;

        time += gameTime.ElapsedGameTime;
        if (time >= Delay)
        {
            time -= Delay;
            currFrame = (currFrame + 1) % sprites.Count;
        }
    }

    public void Draw()
    {
        if (sprites.Count == 0) return;

        string currSpriteName = sprites[currFrame];
        var rect = atlas.GetRect(currSpriteName);
        Core.SpriteBatch.Draw(atlas.GetAtlas(), Position, rect, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
    }

    public void Stop() => IsAnimating = !IsAnimating;

    public void Reset()
    {
        currFrame = 0;
        time = TimeSpan.Zero;
    }

    public Rectangle GetCurrRect() => atlas.GetRect(sprites[currFrame]);
}