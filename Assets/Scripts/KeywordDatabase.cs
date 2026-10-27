using System;
using System.Collections.Generic;
using UnityEngine;

public static class KeywordDatabase
{
    // Using HashSets with case-insensitive comparison so "wood", "Wood", and "WOOD" all match
    public static HashSet<string> Tier1 = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Item", "Creature", "Wood", "Metal", "Round", "Square", "Food", "Tool", "Red", "Blue"
    };

    public static HashSet<string> Tier2 = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Plant", "Machine", "Liquid", "Plastic", "Glass", "Cloth", "Long", "Flat", "Sharp", 
        "Container", "Toy", "Weapon", "Green", "Shiny", "Dark"
    };

    public static HashSet<string> Tier3 = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Furniture", "Electronics", "Clothing", "Animal", "Paper", "Stone", "Leather", "Rubber", 
        "Hollow", "Curved", "Pointy", "Heavy", "Vehicle", "Light", "Cleaning", "Medicine", 
        "Yellow", "White", "Black", "Transparent"
    };

    public static HashSet<string> Tier4 = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "Mineral", "Artwork", "Document", "Chemical", "Decoration", "Ceramic", "Gold", "Organic", 
        "Synthetic", "Crystal", "Spherical", "Cylindrical", "Flexible", "Rigid", "Asymmetrical", 
        "Communication", "Measurement", "Protection", "Entertainment", "Construction", 
        "Glowing", "Patterned", "Matte", "Colorful", "Rough"
    };

    // Master check to see if a keyword exists in ANY tier
    public static bool IsValidKeyword(string word)
    {
        return Tier1.Contains(word) || 
               Tier2.Contains(word) || 
               Tier3.Contains(word) || 
               Tier4.Contains(word);
    }
}