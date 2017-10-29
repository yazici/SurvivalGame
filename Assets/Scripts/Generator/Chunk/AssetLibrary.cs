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

        public GameObject[] Trees => GetTaggedPrefabs("tree");
        public GameObject[] Flowers => GetTaggedPrefabs("flower");
        public GameObject[] Animals => GetTaggedPrefabs("animal");
        public GameObject[] LandAnimals => GetTaggedPrefabs("land", Animals);
        public GameObject[] SnowyTrees => GetTaggedPrefabs("snow", Trees);
        public GameObject[] Cactus => GetTaggedPrefabs("cactus");
        public GameObject[] People => GetTaggedPrefabs("people");
        public GameObject[] Rocks => GetTaggedPrefabs("rock");

        public GameObject[] GetTaggedPrefabs(string tag)
        {
            return GetTaggedPrefabs(tag, prefabs);
        }

        public static GameObject[] GetTaggedPrefabs(string tag, GameObject[] prefabs)
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
                if (taggable.HasTag(tag))
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
