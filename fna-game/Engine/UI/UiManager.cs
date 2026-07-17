using Microsoft.Xna.Framework;

namespace Videogame.Engine.UI;

public static class UiManager
{
    public static List<UiElement> UiElements = new List<UiElement>();
    private static List<UiElement> elemsToAdd = new List<UiElement>();
    private static List<UiElement> elemsToRemove = new List<UiElement>();

    public static T CreateUiElement<T>() where T : UiElement, new()
    {
        var elem = new T();
        elemsToAdd.Add(elem);
        return elem;
    }

    public static void DestroyUiElement(UiElement ui)
    {
        if (!elemsToRemove.Contains(ui) && !ui.isDestroying)
        {
            elemsToRemove.Add(ui);
            ui.Destroy();
        }
    }

    public static void UpdateUi(GameTime gameTime)
    {
        foreach (var ui in elemsToAdd)
        {
            UiElements.Add(ui);
        }
        elemsToAdd.Clear();

        foreach (var ui in elemsToRemove)
        {
            UiElements.Remove(ui);
        }
        elemsToRemove.Clear();

        foreach (var ui in UiElements)
        {
           ui.Update(gameTime);
        }    
    }

    public static void DrawUi()
    {
        foreach (var ui in UiElements)
        {
            ui.Draw();
        }
    }

    public static T? FindUiElement<T>() where T : UiElement
        => UiElements.OfType<T>().FirstOrDefault();
}