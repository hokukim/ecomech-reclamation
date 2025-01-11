using System.Collections.Generic;

namespace EcomechReclamation.Items;

public static class Collectibles
{
    public const string MagicalFlower = "Magical Flower";

    private static Dictionary<string, Collectible> Map => new()
    {
        { MagicalFlower, new Collectible(MagicalFlower, "Magical flower", "img.png") { } }
    };

    public static Collectible Get(string name) => Map[name];
}

public readonly record struct Collectible(string Name, string Description, string Thumbnail);
