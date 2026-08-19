using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Videogame.Engine.Textures;

[JsonSerializable(typeof(Dictionary<string, Dictionary<string, int>>))]
public partial class AtlasJsonContext : JsonSerializerContext {}

public class TextureAtlas
{
    private Texture2D _atlasTexture;
    private Dictionary<string, Rectangle> _spriteRects;

    public TextureAtlas()
    {
        Core.Atlas = this;
    }

    public void LoadAtlas(string atlasName, string jsonName)
    {
        string atlasPath = "fna-game.Content.Graphics." + atlasName;
        string jsonPath = "fna-game.Content.Graphics." + jsonName;

        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(atlasPath))
        {
            if (stream == null)
            {
                throw new FileNotFoundException($"{atlasPath} file was not found!");
            }
            _atlasTexture = Texture2D.FromStream(Core.GraphicsDevice, stream);
        }

        using (var jsonStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(jsonPath))
        {
            if (jsonStream == null)
            {
                throw new FileNotFoundException($"{jsonPath} file was not found!");
            }

            using var reader = new StreamReader(jsonStream);
            var json = reader.ReadToEnd();
            var dict = JsonSerializer.Deserialize(json, AtlasJsonContext.Default.DictionaryStringDictionaryStringInt32);

            _spriteRects = dict.ToDictionary(kv => kv.Key, kv => new Rectangle(kv.Value["X"], kv.Value["Y"], kv.Value["Width"], kv.Value["Height"]));
        }
    }

    public Rectangle GetSpriteRect(string name)
    {
        if (_spriteRects.TryGetValue(name, out var rect))
        {
            return rect;
        }
        
        throw new Exception($"sprite {name} was not found!");
    }

    public void DrawSprite(string name, SpriteOptions spriteOptions)
    {
        var rect = GetSpriteRect(name);
        Core.SpriteBatch.Draw(_atlasTexture, spriteOptions.Position, rect, spriteOptions.Color, spriteOptions.Rotation, spriteOptions.Origin,
        spriteOptions.Scale, spriteOptions.SpriteEffects, spriteOptions.LayerDepth);
    }
}