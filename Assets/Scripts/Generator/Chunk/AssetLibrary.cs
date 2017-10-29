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
        public GameObject[] SnowyTrees => GetTaggedPrefabs("snow", Trees);
        public GameObject[] Cactus => GetTaggedPrefabs("cactus");
        public GameObject[] People => GetTaggedPrefabs("people");

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

        [ContextMenu("Show Snowy Trees")]
        private void ShowTrees()
        {
            var trees = GetTaggedPrefabs("SEA");
            var snowyTrees = GetTaggedPrefabs("snow", trees);

            float x = 0;
            float z = 0;

            foreach (var go in trees)
            {
                Vector3 pos = new Vector3(x, 0, z);
                var o = GameObject.Instantiate(go, pos, Quaternion.identity);

                x += 10f;
                if (x > 100.0f)
                {
                    x = 0;
                    z += 10f;
                }
            }
        }

#endif
    }
}
