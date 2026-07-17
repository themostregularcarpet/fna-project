using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Videogame.Engine.Textures;

public class AnimatedSprite : Sprite
{
    public TimeSpan Delay;
    public bool IsAnimating = true;
    public bool IsLooping = true;
    private int currFrame;
    private TimeSpan time;
    private List<string> sprites = new List<string>();
    private TextureAtlas atlas;

    public AnimatedSprite() {}

    public AnimatedSprite(TextureAtlas atlas, TimeSpan delay, bool isLooping = true, params string[] sprites) 
    {
        this.atlas = atlas;
        this.sprites.AddRange(sprites);
        IsLooping = isLooping;
        Delay = delay;
    }

    public void Update(GameTime gameTime)
    {
        if (!IsAnimating || sprites.Count == 0) return;

        time += gameTime.ElapsedGameTime;
        if (time >= Delay)
        {
            time -= Delay;
            if (currFrame <= sprites.Count)
            {
                currFrame++;
            } else
            {
                if (IsLooping)
                    return;
                
                currFrame = 0;
            }
        }
    }

    public void Draw()
    {
        if (sprites.Count == 0) return;

        string currSpriteName = sprites[currFrame];
        var rect = atlas.GetRect(currSpriteName);
        Core.SpriteBatch.Draw(atlas.GetAtlas(), Position, rect, Color, Rotation, Origin, Scale, SpriteEffects, LayerDepth);
    }

    /// <summary>
    /// switches animation to or not to play, just haven't found a better way to name it :P
    /// </summary>
    public void SwitchAnimatingState() => IsAnimating = !IsAnimating;

    /// <summary>
    /// switches animation to or not to loop, just haven't found a better way to name it :P
    /// </summary>
    public void SwitchAnimationLooping() => IsLooping = !IsLooping;

    public void Reset()
    {
        currFrame = 0;
        time = TimeSpan.Zero;
    }

    public Rectangle GetCurrRect() => atlas.GetRect(sprites[currFrame]);
}