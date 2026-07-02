using Microsoft.Xna.Framework;

namespace Videogame.Engine.Camera;

public class Camera
{
    private Vector2 position;
    private Vector2 smoothPosition;
    private Vector2 targetPosition;

    public Vector2 Position
    {
        get => position;
        set => targetPosition = value;
    }

    public Vector2 Origin { get; set; }
    public float Zoom { get; set; } = 1f;
    public float Rotation { get; set; } = 0f;

    public Camera(Vector2 origin)
    {
        Origin = origin;
        position = Vector2.Zero;
        smoothPosition = Vector2.Zero;
        targetPosition = Vector2.Zero;
    }

    public Matrix GetViewMatrix()
    {
        return Matrix.CreateTranslation(new Vector3(-position, 0f)) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(Zoom, Zoom, 1f)) *
               Matrix.CreateTranslation(new Vector3(Origin, 0f));
    }

    public void Update(int width, int height, GameTime gameTime)
    {
        float halfWidth = Origin.X;
        float halfHeight = Origin.Y;

        float targetX = targetPosition.X;
        float targetY = targetPosition.Y;

        if (width < halfWidth * 2)
            targetX = width / 2f;
        else
            targetX = Math.Clamp(targetX, halfWidth, width - halfWidth);

        if (height < halfHeight * 2)
            targetY = height / 2f;
        else
            targetY = Math.Clamp(targetY, halfHeight, height - halfHeight);

        if (smoothPosition == Vector2.Zero)
        {
            smoothPosition = new Vector2(targetX, targetY);
        }

        float lerpFactor = 1f - (float)Math.Pow(0.00075f, gameTime.ElapsedGameTime.TotalSeconds);
        smoothPosition.X += (targetX - smoothPosition.X) * lerpFactor;
        smoothPosition.Y += (targetY - smoothPosition.Y) * lerpFactor;

        position = new Vector2(MathF.Round(smoothPosition.X), MathF.Round(smoothPosition.Y));
    }
}