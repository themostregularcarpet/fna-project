using Microsoft.Xna.Framework;
using Videogame.Engine.Camera;

namespace Videogame.Engine.Cutscene;

public class MoveCameraCommand : TimedCommand
{
    private Vector2 start;
    private Vector2 end;

    public MoveCameraCommand(Vector2 target, float duration) : base(duration)
    {
        end = target;
    }

    public override void Start()
    {
        base.Start();
        start = Core.Camera.Position;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        float t = Elapsed / Duration;
        Core.Camera.Position = Vector2.Lerp(start, end, t);
    }
}