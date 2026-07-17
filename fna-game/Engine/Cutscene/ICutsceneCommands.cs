using Microsoft.Xna.Framework;

namespace Videogame.Engine.Cutscene;

public interface ICutsceneCommands
{
    bool IsCompleted { get; }
    void Update(GameTime gameTime);
    void Start();
    void Skip();
}