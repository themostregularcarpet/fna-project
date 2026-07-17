using FontStashSharp;

namespace Videogame.Engine.UI;

public class Fonts
{
    private static FontSystem fontSystem;
    public static SpriteFontBase MainFont;

    public void Load()
    {
        FontSystemDefaults.FontResolutionFactor = 4.0f;
        FontSystemDefaults.KernelHeight = 4;
        FontSystemDefaults.KernelWidth = 4;
        var fullPath = Path.Combine(Core.Content.RootDirectory, "Fonts", "mainFont.otf");
        fontSystem = new FontSystem(new FontSystemSettings
        {
            TextureWidth = 1024,
            TextureHeight = 1024,
            KernelHeight = 1,
            KernelWidth = 1
        });

        fontSystem.AddFont(File.ReadAllBytes(fullPath));
        MainFont = fontSystem.GetFont(32);
    }
}