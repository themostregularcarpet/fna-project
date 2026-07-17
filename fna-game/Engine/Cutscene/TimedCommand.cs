using Microsoft.Xna.Framework;

namespace Videogame.Engine.Cutscene;

public abstract class TimedCommand : ICutsceneCommands
{
    protected float Duration;
    protected float Elapsed;

    public bool IsCompleted => Elapsed >= Duration;

    public TimedCommand(float duration) => Duration = duration;

    public virtual void Start() => Elapsed = 0f;

    public virtual void Update(GameTime gameTime)
    {
        Elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (Elapsed > Duration)
            Elapsed = Duration;
    }

    public virtual void Skip() => Elapsed = Duration;
}