using Microsoft.Xna.Framework;

namespace Videogame.Engine.Textures;

public class Animation
{
    private int currFrame = 0;
    private bool isAnimating = true;
    private List<string> frames = new();
    private TimeSpan delay;
    private TimeSpan elapsedTime = TimeSpan.Zero;

    public Animation(TimeSpan delay, params string[] frames)
    {
        this.frames.AddRange(frames);
        this.delay = delay;
    }

    public void Update(GameTime gameTime)
    {
        if (isAnimating)
        {
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime >= delay)
            {
                if (currFrame < frames.Count - 1)
                {
                    currFrame++;
                } 
                else
                {
                    currFrame = 0;    
                }
                elapsedTime -= delay;
            }       
        }
    }

    public void Draw(SpriteOptions spriteOptions)
    {
        var frameToDraw = frames[currFrame];
        Core.Atlas.DrawSprite(frameToDraw, spriteOptions);
    }

    public void Reset()
    {
        currFrame = 0;
        elapsedTime = TimeSpan.Zero;
    }

    public void ToggleAnimation() => isAnimating = !isAnimating;

    public int GetCurrentFrame() => currFrame;
}