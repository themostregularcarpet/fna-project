using Microsoft.Xna.Framework.Input;

namespace Videogame.Engine.Input;

public class KeyboardInput
{
    private KeyboardState prevState;
    private KeyboardState currState;

    public KeyboardInput()
    {
        prevState = new KeyboardState();
        currState = Keyboard.GetState();
    }

    public void Update()
    {
        prevState = currState;
        currState = Keyboard.GetState();
    }

    public bool IsKeyDown(Keys key) => currState.IsKeyDown(key);

    public bool IsKeyUp(Keys key) => currState.IsKeyUp(key);

    public bool WasKeyPressed(Keys key) => currState.IsKeyDown(key) && prevState.IsKeyUp(key);

    public bool WasKeyReleased(Keys key) => currState.IsKeyUp(key) && prevState.IsKeyDown(key);
}