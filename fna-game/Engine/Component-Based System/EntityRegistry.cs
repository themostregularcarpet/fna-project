
using System.Reflection;

namespace Videogame.Engine.CBS;

public static class EntityRegistry
{
    private static Dictionary<string, Type> entityTypes;

    public static void Init()
    {
        entityTypes = new Dictionary<string, Type>();
        var assembly = Assembly.GetExecutingAssembly();
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsSubclassOf(typeof(Entity)) && !type.IsAbstract)
            {
                entityTypes[type.Name] = type;
            }
        }
    }

    public static Entity Create(string name)
    {
        string entityName = char.ToUpper(name[0]) + name.Substring(1);
        if (entityTypes.TryGetValue(entityName, out var type))
        {
            return (Entity)Activator.CreateInstance(type);
        }
        throw new Exception($"entity {entityName} not registered");
    }
}