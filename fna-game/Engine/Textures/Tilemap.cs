using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Videogame.Engine.Textures;

[JsonSerializable(typeof(Level))]
[JsonSerializable(typeof(Layer))]
public partial class LevelContext : JsonSerializerContext {}

public class Level
{
    public int Width { get; set; }
    public int Height { get; set; }
    [JsonPropertyName("layers")]
    public List<Layer> Layers { get; set; }
}

public class Layer
{
    public string Name { get; set; }
    public int GridCellWidth { get; set; }
    public int GridCellHeight { get; set; }
    public int GridCellsX { get; set; }
    public int GridCellsY { get; set; }
    public List<int> Data { get; set; }
    public List<Entity> Entities { get; set; }
}

public class Entity
{
    public string Name { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public class Tilemap
{
    public int RoomWidth => levelWidth * tileWidth;
    public int RoomHeight => levelHeight * tileHeight;

    private int[,] tiles;
    private int levelWidth;
    private int levelHeight;
    private int tileWidth;
    private int tileHeight;
    private string name;
    private Texture2D tileset;
    private List<Layer> layers;
    
    private string collisionLayerName;
    private List<Rectangle> tileRects;
    private List<Rectangle> collisionRects = new List<Rectangle>();

    private List<Entity> entities = new List<Entity>();
    private string entityLayerName;

    public Tilemap(string mapName, string tilesetName, string? collisionLayerName, string? entityLayerName)
    {
        this.collisionLayerName = collisionLayerName;
        this.entityLayerName = entityLayerName;

        string mapPathExt = Path.Combine(Core.Content.RootDirectory, "Graphics", mapName + ".json");
        string json = File.ReadAllText(mapPathExt);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            TypeInfoResolver = LevelContext.Default
        };
        Level level = JsonSerializer.Deserialize<Level>(json, options);
        
        layers = level.Layers; 
        
        var layer = level.Layers[0];
        name = layer.Name;
        tileWidth = layer.GridCellWidth;
        tileHeight = layer.GridCellHeight;
        levelWidth = layer.GridCellsX;
        levelHeight = layer.GridCellsY;

        tileRects = new List<Rectangle>();
        string fullPath = Path.Combine(Core.Content.RootDirectory, "Graphics", tilesetName + ".png");

        using (var fs = File.OpenRead(fullPath))
        using (var ms = new MemoryStream())
        {
            fs.CopyTo(ms);
            ms.Position = 0;
            tileset = Texture2D.FromStream(Core.GraphicsDevice, ms);
        }

        int tilesPerRow = tileset.Width / tileWidth;
        int totalTiles = (tileset.Width / tileWidth) * (tileset.Height / tileHeight);
        for (int i = 0; i < totalTiles; i++)
        {
            int x = (i % tilesPerRow) * tileWidth;
            int y = (i / tilesPerRow) * tileHeight;
            tileRects.Add(new Rectangle(x, y, tileWidth, tileHeight));
        }

        var collisionLayer = level.Layers.FirstOrDefault(l => l.Name == collisionLayerName);
        if (collisionLayer != null)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    int index = y * levelWidth + x;
                    int tileId = collisionLayer.Data[index];
                    
                    if (tileId != -1)
                    {
                        var rect = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);
                        collisionRects.Add(rect);
                    }
                }
            }
        }
        Actor.TileRects = collisionRects;

        var entityLayer = level.Layers.FirstOrDefault(l => l.Name == entityLayerName);
        if (entityLayer != null)
            entities = entityLayer.Entities ?? new List<Entity>();
        else
            entities = new List<Entity>();

        foreach (var entity in entities)
        {
            var position = new Vector2(entity.X, entity.Y);
            ActorManager.CreateActorByName(entity.Name, position);
        }
    }

    public void Draw()
    {
        foreach (var layer in layers)
        {
            if (layer.Data != null)
            {
                if (layer.Name != collisionLayerName)
                {
                    for (int y = 0; y < levelHeight; y++)
                    {
                        for (int x = 0; x < levelWidth; x++)
                        {
                            int index = y * levelWidth + x;
                            int tileId = layer.Data[index];

                            if (tileId != -1)
                            {
                                if (tileId > 0 || tileId <= tileRects.Count)
                                {
                                    var rect = tileRects[tileId];
                                    Vector2 position = new Vector2(x * tileWidth, y * tileHeight);
                                    Core.SpriteBatch.Draw(tileset, position, rect, Color.White);                                    
                                } 
                            }
                        }
                    }
                }
            }
        }
    }
}