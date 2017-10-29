using Pamux.Utilities;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pamux.Lib.LevelData.Generator
{
    public class AssetLibrary : Singleton<AssetLibrary>
    {
        [SerializeField]
        private GameObject[] prefabs;

        public GameObject[] Trees => GetTaggedPrefabs(new string[] { "tree" }, new string[] { "leaf" });
        public GameObject[] Flowers => GetTaggedPrefabs(new string[] { "flower" }, new string[] { });
        public GameObject[] Animals => GetTaggedPrefabs(new string[] { "animal" }, new string[] { });
        public GameObject[] LandAnimals => GetTaggedPrefabs(new string[] { "animal", "land" }, new string[] { });
        public GameObject[] SnowyTrees => GetTaggedPrefabs(new string[] { "snow" }, new string[] { }, Trees);
        public GameObject[] Cactus => GetTaggedPrefabs(new string[] { "cactus" }, new string[] { });
        public GameObject[] People => GetTaggedPrefabs(new string[] { "people" }, new string[] { });
        public GameObject[] Rocks => GetTaggedPrefabs(new string[] { "rock", "stone" }, new string[] { });

        public GameObject[] GetTaggedPrefabs(string[] withTags, string[] butNotWithTags)
        {
            return GetTaggedPrefabs(withTags, butNotWithTags, prefabs);
        }

        public static GameObject[] GetTaggedPrefabs(string[] withTags, string[] butNotWithTags, GameObject[] prefabs)
        {
            var result = new List<GameObject>();
            if (prefabs == null)
            {
                return result.ToArray();
            }
            
            foreach (var prefab in prefabs)
            {
                if (prefab == null)
                {
                    continue;
                }
                var taggable = prefab.GetComponent<Taggable>();
                if (taggable == null)
                {
                    continue;
                }

                if (taggable.HasAnyTag(withTags) && !taggable.HasAnyTag(butNotWithTags))
                {
                    result.Add(prefab);
                }
            }

            return result.ToArray();
        }

#if UNITY_EDITOR

        [ContextMenu("Tag Prefabs")]
        private void TagPrefabs()
        {
            var guids = AssetDatabase.FindAssets(@"");
            if (guids == null)
            {
                return ;
            }
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!path.EndsWith(".prefab"))
                {
                    continue;
                }
                if (!path.StartsWith("Assets/3rdParty"))
                {
                    continue;
                }
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    continue;
                }

                var taggable = prefab.GetComponent<Taggable>();
                if (taggable == null)
                {
                    taggable = prefab.AddComponent<Taggable>();
                }
                
                taggable.EnsureTag("lowpoly");

                taggable.TagFromPath(path.ToLowerInvariant());
            }
        }

        private GameObject[] GetAllPrefabs()
        {
            var prefabs = new List<GameObject>();

            var guids = AssetDatabase.FindAssets(@"");
            if (guids == null)
            {
                return prefabs.ToArray();
            }
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!path.EndsWith(".prefab"))
                {
                    continue;
                }
                if (!path.StartsWith("Assets/3rdParty"))
                {
                    continue;
                }
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab == null)
                {
                    continue;
                }

                prefabs.Add(prefab);
            }

            return prefabs.ToArray();
        }


        [ContextMenu("Attach Prefabs")]
        private void AttachPrefabs()
        {
            prefabs = GetAllPrefabs();
        }

#endif
    }
}
