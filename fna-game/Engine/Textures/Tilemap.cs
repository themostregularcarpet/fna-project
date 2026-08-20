using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Videogame.Engine.CBS;
using System.Reflection;

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
    public List<TilemapEntity> Entities { get; set; }
}

public class TilemapEntity
{
    public string Name { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
}

public class Tilemap
{
    public int RoomWidth => levelWidth * tileWidth;
    public int RoomHeight => levelHeight * tileHeight;

    private int levelWidth;
    private int levelHeight;
    private int tileWidth;
    private int tileHeight;
    private Texture2D tileset;
    private List<Layer> layers;
    
    private string collisionLayerName;
    private List<Rectangle> tileRects;
    private List<Rectangle> collisionRects = new List<Rectangle>();

    private List<TilemapEntity> entities = new List<TilemapEntity>();
    private List<Entity> createdEntities = new List<Entity>();

    public Tilemap(string mapName, string tilesetName, string collisionLayerName = null, string entityLayerName = null)
    {
        this.collisionLayerName = collisionLayerName;

        string fullMapPath = "fna-game.Content.Graphics." + mapName + ".json";
        Level level;

        using (var jsonStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullMapPath))
        {
            if (jsonStream == null)
            {
                throw new FileNotFoundException($"{fullMapPath} file was not found!");
            }

            using var reader = new StreamReader(jsonStream);
            var json = reader.ReadToEnd();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                TypeInfoResolver = LevelContext.Default
            };
            
            Level lvl = JsonSerializer.Deserialize<Level>(json, options);
            if (lvl == null) return;
            level = lvl;
        }
        
        layers = level.Layers; 
        var layer = level.Layers[0];
        tileWidth = layer.GridCellWidth;
        tileHeight = layer.GridCellHeight;
        levelWidth = layer.GridCellsX;
        levelHeight = layer.GridCellsY;

        tileRects = new List<Rectangle>();
        string fullTilesetPath = "fna-game.Content.Graphics." + tilesetName + ".png";

        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullTilesetPath))
        {
            if (stream == null)
            {
                throw new FileNotFoundException($"{fullTilesetPath} file was not found!");
            }
            tileset = Texture2D.FromStream(Core.GraphicsDevice, stream);
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
        PhysicsComponent.TileRects = collisionRects;

        var entityLayer = level.Layers.FirstOrDefault(l => l.Name == entityLayerName);
        if (entityLayer != null)
        {
            entities = entityLayer.Entities ?? new List<TilemapEntity>();
        }

        foreach (var entity in entities)
        {
            var createdEntity = Scene.AddEntity(entity.Name);
            createdEntities.Add(createdEntity);
            foreach (var e in createdEntities)
            {
                var transform = e.GetComponent<TransformComponent>();
                if (transform != null)
                {
                    transform.Position = new Vector2(entity.X, entity.Y);
                }
            }
        }
    }

    public void Draw()
    {
        foreach (var layer in layers)
        {
            if (layer.Data != null && layer.Name != collisionLayerName)
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
            else
            {
                foreach (var e in createdEntities)
                {
                    var drawableComponent = e.GetComponent<SpriteComponent>();

                    if (drawableComponent != null)
                    {
                        drawableComponent?.Draw();
                    }  
                }
            }
        }
    }

    public void Unload()
    {
        tileset?.Dispose();
    }
}