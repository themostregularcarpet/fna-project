using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Videogame.Engine.Input;

public class Input
{
    public KeyboardInput Keyboard;
    public GamepadInput[] Gamepad;

    public Input()
    {
        Keyboard = new KeyboardInput();

        Gamepad = new GamepadInput[4];

        for (int i = 0; i < 4; i++)
        {
            Gamepad[i] = new GamepadInput((PlayerIndex)i);
        }
    }

    public void Update(GameTime gameTime)
    {
        Keyboard.Update();

        for (int i = 0; i < 4; i++)
        {
            Gamepad[i].Update(gameTime);
        }
    }
}