using Microsoft.Xna.Framework;

namespace Videogame.Engine.CBS;

public class CameraComponent
{
    private Vector2 _position;
    private Vector2 _smoothPosition;    
    private Vector2 _targetPosition;

    public Vector2 Position
    {
        get => _position;
        set => _targetPosition = value;
    }
    public Vector2 Origin { get; set; }
    public float Zoom { get; set; } = 1f;
    public float Rotation { get; set; } = 0f;

    public CameraComponent(Vector2 origin)
    {
        Origin = origin;
        _position = Vector2.Zero;
        _smoothPosition = Vector2.Zero;
        _targetPosition = Vector2.Zero;
    }

    public Matrix GetViewMatrix()
    {
        return Matrix.CreateTranslation(new Vector3(-_position, 0f)) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(Zoom, Zoom, 1f)) *
               Matrix.CreateTranslation(new Vector3(Origin, 0f));
    }

    public void Update(int clampWidth, int clampHeight, GameTime gameTime)
    {
        float halfWidth = Origin.X;
        float halfHeight = Origin.Y;

        float targetX = _targetPosition.X;
        float targetY = _targetPosition.Y;

        if (clampWidth < halfWidth * 2)
        {
            targetX = clampWidth / 2f;
        }
        else
        {
            targetX = Math.Clamp(targetX, halfWidth, clampWidth - halfWidth);
        }

        if (clampHeight < halfHeight * 2)
        {
            targetY = clampHeight / 2f;
        }
        else
        {
            targetY = Math.Clamp(targetY, halfHeight, clampHeight - halfHeight);
        }

        if (_smoothPosition == Vector2.Zero)
        {
            _smoothPosition = new Vector2(targetX, targetY);
        }

        float lerpFactor = 1f - (float)Math.Pow(0.00075f, gameTime.ElapsedGameTime.TotalSeconds);
        _smoothPosition.X += (targetX - _smoothPosition.X) * lerpFactor;
        _smoothPosition.Y += (targetY - _smoothPosition.Y) * lerpFactor;

        _position = new Vector2(MathF.Round(_smoothPosition.X), MathF.Round(_smoothPosition.Y));
    }
}