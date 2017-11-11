using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pamux.Lib.GameObjects
{
    public class Taggable : MonoBehaviour
    {
        public bool ShouldUse => !DontUseAsTagged && Opinion != "no";
        public bool DontUseAsTagged = false;
        public string Opinion;
        public string PrefabAssetPath; 

        [SerializeField]
        public WeightedTag[] WeightedTags;
        
        internal void EnsureTag(string tagStr)
        {
            var weightedTag = new WeightedTag { Tag = tagStr, Weight = 1.0f };

            if (WeightedTags == null)
            {
                WeightedTags = new WeightedTag[] { weightedTag };
                return;
            }

            if (HasTag(tagStr))
            {
                return;
            }

            var newTags = new List<WeightedTag>();
            newTags.AddRange(WeightedTags);
            newTags.Add(weightedTag);
            WeightedTags = newTags.ToArray();
        }

        internal void TagFromPath(string path)
        {
            string[] tags = {
            "tree",
            "log",
            "plank",
            "stump",
            "rock",
            "red",
            "green",
            "blue",
            "yellow",
            "grey",
            "NotAnimated",
            "WithController",
            "CharactersAnimated",
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
            "characters",
            "flag",
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

            if (WeightedTags.Length < 2)
            {
                Debug.Log(path);
            }
        }

        internal bool HasAnyTag(string[] tags)
        {
            foreach (var tag in tags)
            {
                if (HasTag(tag))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasTag(string tag)
        {
            foreach (var aTag in WeightedTags)
            {
                if (aTag.Tag != null && aTag.Tag.Equals(tag, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}