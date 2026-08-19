using System.Drawing;
using System.Drawing.Imaging;
using System.IO.Compression;
using System.Text.Json;

namespace texturepacker;

public class Program
{
    static void Main(string[] args)
    {
        /*
        if (args.Length < 3)
        {
            Console.WriteLine("<input_folder> <output_image> <output_json>");
            return;
        }
        */

        string inputFolder = "../TexturePacker/Assets";
        string outputImage = "atlas.png";
        string outputJson = "atlas_data.json";

        if (!Directory.Exists(inputFolder))
        {
            Console.WriteLine("no such directory.");
            return;
        }

        var files = Directory.GetFiles(inputFolder, "*.png");
        if (files.Length == 0)
        {
            Console.WriteLine("no pngs found.");
            return;
        }

        var bitmaps = new List<(string Name, Bitmap Image)>();
        foreach (var file in files)
        {
            try
            {
                var bmp = new Bitmap(file);
                bitmaps.Add((Path.GetFileNameWithoutExtension(file), bmp));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"failed to load {file}: {ex.Message}");
                return;
            }
        }

        int totalArea = bitmaps.Sum(b => b.Image.Width * b.Image.Height);
        int atlasSize = 256;
        while (atlasSize * atlasSize < totalArea * 2)
        {
            atlasSize *= 2;
        }
        atlasSize = Math.Min(atlasSize, 8192);

        var freeRects = new List<Rectangle> { new Rectangle(0, 0, atlasSize, atlasSize) };
        var busyRects = new List<(string Name, Rectangle Rect)>();

        var sorted = bitmaps.OrderByDescending(b => b.Image.Width * b.Image.Height).ToList();

        foreach (var (name, bmp) in sorted)
        {
            var result = FindBestFit(freeRects, bmp.Width, bmp.Height);
            if (result == null)
            {
                Console.WriteLine($"cannot fit '{name}' into atlas.");
                return;
            }

            var (freeRect, placedRect) = result.Value;
            busyRects.Add((name, placedRect));
            SplitFreeRects(freeRects, freeRect, placedRect);
        }

        using var atlas = new Bitmap(atlasSize, atlasSize, PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(atlas);
        g.Clear(Color.Transparent);

        foreach (var (name, rect) in busyRects)
        {
            var bmp = bitmaps.First(b => b.Name == name).Image;
            g.DrawImage(bmp, rect.X, rect.Y, rect.Width, rect.Height);
        }

        atlas.Save(outputImage, ImageFormat.Png);
        Console.WriteLine($"atlas saved to {outputImage}");

        var data = busyRects.ToDictionary(
            p => p.Name,
            p => new { p.Rect.X, p.Rect.Y, p.Rect.Width, p.Rect.Height }
        );
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(outputJson, json);
        Console.WriteLine($"JSON saved to {outputJson}");

        foreach (var (_, bmp) in bitmaps)
        {
            bmp.Dispose();
        }

        using (var fileStream = File.OpenRead("atlas.png"));
    }

    static (Rectangle freeRect, Rectangle placedRect)? FindBestFit(List<Rectangle> freeRects, int width, int height)
    {
        Rectangle? bestFree = null;
        int bestArea = int.MaxValue;

        foreach (var rect in freeRects)
        {
            if (rect.Width >= width && rect.Height >= height)
            {
                int area = rect.Width * rect.Height;
                if (area < bestArea)
                {
                    bestArea = area;
                    bestFree = rect;
                }
            }
        }

        if (bestFree == null)
            return null;

        var placed = new Rectangle(bestFree.Value.X, bestFree.Value.Y, width, height);
        return (bestFree.Value, placed);
    }

    static void SplitFreeRects(List<Rectangle> freeRects, Rectangle freeRect, Rectangle placedRect)
    {
        freeRects.Remove(freeRect);

        if (freeRect.X + placedRect.Width < freeRect.X + freeRect.Width)
        {
            var right = new Rectangle(
                freeRect.X + placedRect.Width,
                freeRect.Y,
                freeRect.Width - placedRect.Width,
                placedRect.Height
            );
            freeRects.Add(right);
        }

        if (freeRect.Y + placedRect.Height < freeRect.Y + freeRect.Height)
        {
            var bottom = new Rectangle(
                freeRect.X,
                freeRect.Y + placedRect.Height,
                placedRect.Width,
                freeRect.Height - placedRect.Height
            );
            freeRects.Add(bottom);
        }
    }
}