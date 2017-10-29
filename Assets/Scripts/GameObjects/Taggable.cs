using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taggable : MonoBehaviour
{
    public string[] Tags;

    internal void EnsureTag(string tag)
    {
        if (Tags == null)
        {
            Tags = new string[] { tag };
            return;
        }

        if (HasTag(tag))
        {
            return;
        }

        var newTags = new List<string>();
        newTags.AddRange(Tags);
        newTags.Add(tag);
        Tags = newTags.ToArray();
    }

    internal void TagFromPath(string path)
    {
        string[] tags = {
            "tree",
            "rock",
            "snow",
            "winter",
            "summer",
            "platform",
            "cliff",
            "cactus",
            "plant",
            "animal",
            "food",
            "bush",
            "grass",
            "flower",
            "people",
            "woman",
            "crop",
            "structure",
            "house",
            "fire",
            "stone",
            "tool",
            "vehicle",
            "building",
            "rail",
            "train",
            "boat",
            "pickup",
            "spring",
            "gem",
            "particle",
            "effect",
            "beach",
            "mushroom",
            "rain",
            "sun",
            "moon",
            "balloon",
            "fence",
            "props",
            "ice",
            "leaf",
            "tropical",
            "chest",
            "statue",
            "cave",
            "crate",
            "barrel",
            "torch",
            "wildwest",
            "light",
            "fall",
            "autumn",
            "castle",
            "town",
            "city",
            "scenery",
            "bone",
            "desert",
            "garden",
            "carrot",
            "corn",
            "plateau",
            "tent",
            "bridge",
            "path",
            "lake",
            "mountain",
            "cliff",
            "pick-up",
            "sea",
            "water",
            "nature",
            "cloud",
            "terrain"};

        foreach (var tag in tags)
        {
            if (path.Contains(tag))
            {
                EnsureTag(tag);
            }
        }

        if (Tags.Length < 2)
        {
            Debug.Log(path);
        }
    }

    public bool HasTag(string tag)
    {
        foreach (var aTag in Tags)
        {
            if (aTag.Equals(tag, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}
