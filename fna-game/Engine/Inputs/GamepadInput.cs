using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Videogame.Engine.Input;

public class GamepadInput
{
    public bool IsConnected => currState.IsConnected;
    public Vector2 LeftThumb => currState.ThumbSticks.Left;
    public Vector2 RightThumb => currState.ThumbSticks.Right;
    public PlayerIndex PlayerIndex { get; }

    private TimeSpan vibrationTimeRemaining = TimeSpan.Zero;
    private GamePadState prevState;
    private GamePadState currState;

    public GamepadInput(PlayerIndex playerId)
    {
        PlayerIndex = playerId;
        prevState = new GamePadState();
        currState = GamePad.GetState(playerId);
    }

    public void Update(GameTime gameTime)
    {
        prevState = currState;
        currState = GamePad.GetState(PlayerIndex);

        if (vibrationTimeRemaining > TimeSpan.Zero)
        {
            vibrationTimeRemaining -= gameTime.ElapsedGameTime;

            if (vibrationTimeRemaining <= TimeSpan.Zero)
            {
                StopVibration();
            }
        }
    }

    public bool IsButtonDown(Buttons button) => currState.IsButtonDown(button);

    public bool IsButtonUp(Buttons button) => currState.IsButtonUp(button);

    public bool WasButtonPressed(Buttons button) => currState.IsButtonDown(button) && prevState.IsButtonUp(button);

    public bool WasButtonReleased(Buttons button) => currState.IsButtonUp(button) && prevState.IsButtonDown(button);

    public void SetVibration(float strength, TimeSpan time)
    {
        vibrationTimeRemaining = time;
        GamePad.SetVibration(PlayerIndex, strength, strength);
    }

    public void StopVibration()
    {
        GamePad.SetVibration(PlayerIndex, 0.0f, 0.0f);
    }
}