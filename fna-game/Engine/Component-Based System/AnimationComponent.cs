using Videogame.Engine.Textures;
using Microsoft.Xna.Framework;

namespace Videogame.Engine.CBS;

public class AnimationComponent : IComponent
{
    public Entity Owner { get; set; }
    public Animation Animation;
    public SpriteOptions SpriteOptions;
    public bool IsVisible = true;

    public AnimationComponent(SpriteOptions so, TimeSpan delay, params string[] frames)
    {
        SpriteOptions = so;
        Animation = new Animation(delay, frames);
    }

    public void Update(GameTime gameTime)
    {
        var transform = Owner.GetComponent<TransformComponent>();
        if (transform != null)
        {
            SpriteOptions.Position = transform.Position;
            SpriteOptions.Rotation = transform.Rotation;
            SpriteOptions.Scale = transform.Scale;
        }
        Animation.Update(gameTime);
    }

    public void Draw()
    {
        if (IsVisible)
        {
            Animation.Draw(SpriteOptions);
        }
    }

    public void ChangeAnimationSprite(Animation animation)
    {
        Animation = animation;
    }
}