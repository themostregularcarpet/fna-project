using System.Data;
using System.Globalization;
using Microsoft.Xna.Framework;

namespace Videogame.Engine;

public static class ActorManager
{
    public static List<Actor> Actors = new();
    private static List<Actor> actorsToAdd = new();
    private static List<Actor> actorsToRemove = new();

    public static T CreateActor<T>() where T : Actor, new()
    {
        var actor = new T();
        actorsToAdd.Add(actor);
        actor.Create();
        if (string.IsNullOrEmpty(actor.Name))
            actor.Name = actor.GetType().Name;
        return actor;
    }

    public static Actor CreateActorByName(string name, Vector2? position)
    {
        string className = char.ToUpper(name[0]) + name.Substring(1);
        string fullName = $"Videogame.Game.Actors.{className}";
        
        Type type = Type.GetType(fullName, true);
        var actor = (Actor)Activator.CreateInstance(type);
        if (position != null)
        {
            actor.Position = (Vector2)position;
        }
        actor.Create();
        actorsToAdd.Add(actor);
        return actor;
    }

    public static void DestroyActor(Actor actor)
    {
        if (!actorsToRemove.Contains(actor) && !actor.IsDestroyed)
        {
            actorsToRemove.Add(actor);
            actor.Destroy();
        }
    }

    public static void UpdateAllActors(GameTime gameTime)
    {
        foreach (var actor in actorsToRemove)
        {
            Actors.Remove(actor);   
        }
        actorsToRemove.Clear();

        foreach (var actor in actorsToAdd)
        {
            Actors.Add(actor);
        }
        actorsToAdd.Clear();

        foreach (var actor in Actors)
        {
            actor.Step(gameTime);
        }
    }

    public static void DrawActors()
    {
        Actors.Sort((a, b) =>
        {
            return a.Position.Y.CompareTo(b.Position.Y);
        });

        foreach (var actor in Actors)
        {
            actor.Draw();
        }
    }

    public static void RemoveActors()
    {
        foreach (var actor in Actors)
        {
            if (!actor.IsPersistent)
            {
                DestroyActor(actor);
            }
            
        }
    }

    public static T? ActorFindByType<T>() where T : Actor
        => Actors.OfType<T>().FirstOrDefault();

    public static Actor? ActorFindByName(string name)
        => Actors.FirstOrDefault(a => a.Name == name);

    public static int ActorCount<T>() where T : Actor => Actors.Count(a => a.GetType() == typeof(T));
}