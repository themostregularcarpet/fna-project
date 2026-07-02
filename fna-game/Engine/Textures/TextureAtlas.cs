using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Videogame.Engine.Textures;

[JsonSerializable(typeof(Dictionary<string, SpriteData>))]
public partial class AtlasDataContext : JsonSerializerContext
{
}

public struct SpriteData
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

public class TextureAtlas : IDisposable
{
    private Texture2D atlas;
    private Dictionary<string, SpriteData> spritesInfo;
    private List<Rectangle> tileRects;
    
    private bool disposed = false;

    public TextureAtlas(string atlasName, string jsonName)
    {
        byte[] atlasBytes = Decompress(atlasName);
        using var memStream = new MemoryStream(atlasBytes);
        atlas = Texture2D.FromStream(Core.GraphicsDevice, memStream);

        string jsonPath = Path.Combine(Core.Content.RootDirectory, "Graphics", jsonName);
        string json = File.ReadAllText(jsonPath);

        spritesInfo = JsonSerializer.Deserialize(json, AtlasDataContext.Default.DictionaryStringSpriteData);
    }

    private static byte[] Decompress(string fileName)
    {
        using var fs = File.OpenRead(Path.Combine(Core.Content.RootDirectory, "Graphics", fileName));
        using var gzip = new GZipStream(fs, CompressionMode.Decompress);
        using var output = new MemoryStream();
        gzip.CopyTo(output);
        return output.ToArray();
    }

    public Rectangle GetRect(string name)
    {
        var data = spritesInfo[name];
        return new Rectangle(data.X, data.Y, data.Width, data.Height);
    }

    public Texture2D GetAtlas() => atlas;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                atlas?.Dispose();
                spritesInfo?.Clear();
                spritesInfo = null;
            }

            disposed = true;
        }
    }
}