using Pamux.Utilities;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.GameObjects;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;
using Assets.Scripts.Generator;
using Assets.Scripts.Utilities;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pamux.Lib.WorldGen
{
    public class AssetLibrary : Singleton<AssetLibrary>
    {
        public static AssetOpinions AssetOpinions = new AssetOpinions();

        [SerializeField]
        public GameObject[] prefabs;

        public GameObject[] Trees => GetTaggedPrefabs(new string[] { "tree" }, new string[] { "leaf", "log", "stump", "branch" });
        public GameObject[] Flowers => GetTaggedPrefabs(new string[] { "flower" }, new string[] { });
        public GameObject[] Animals => GetTaggedPrefabs(new string[] { "animal" }, new string[] { });
        public GameObject[] LandAnimals => GetTaggedPrefabs(new string[] { "animal", "land" }, new string[] { });
        public GameObject[] SnowyTrees => GetTaggedPrefabs(new string[] { "snow" }, new string[] { }, Trees);
        public GameObject[] Fire => GetTaggedPrefabs(new string[] { "fire" }, new string[] { });
        public GameObject[] GreenFire => GetTaggedPrefabs(new string[] { "green" }, new string[] { }, Fire);
        public GameObject[] Cactus => GetTaggedPrefabs(new string[] { "cactus" }, new string[] { });
        public GameObject[] People => GetTaggedPrefabs(new string[] { "people" }, new string[] { });
        public GameObject[] Rocks => GetTaggedPrefabs(new string[] { "rock", "stone" }, new string[] { });
        public GameObject[] Houses => GetTaggedPrefabs(new string[] { "house" }, new string[] { });

        public GameObject[] GetTaggedPrefabs(string[] withTags, string[] butNotWithTags)
        {
            return GetTaggedPrefabs(withTags, butNotWithTags, prefabs);
        }

        public static void DoForEachTaggedPrefab(GameObject[] prefabs, Action<GameObject, Taggable> action)
        {
            var result = new List<GameObject>();
            if (prefabs == null)
            {
                return;
            }

            foreach (var prefab in prefabs)
            {
                if (prefab == null)
                {
                    continue;
                }
                var taggable = prefab.GetComponent<Taggable>();
                if (taggable != null)
                {
                    action.Invoke(prefab, taggable);
                }
            }
        }
        private static void LoadUserDecisions()
        {
            var path = Application.persistentDataPath + "/assetYesNo.json";
            Debug.Log(path);

            var serializer = new DataContractJsonSerializer(typeof(AssetOpinions));

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                AssetLibrary.AssetOpinions = serializer.ReadObject(stream) as AssetOpinions;
            }
        }

        


        public static void DoForEachTaggedPrefab(string[] withTags, string[] butNotWithTags, GameObject[] prefabs, Action<GameObject, Taggable> action)
        {
            DoForEachTaggedPrefab(prefabs, (prefab, taggable) => {
                if (taggable.HasAnyTag(withTags) && !taggable.HasAnyTag(butNotWithTags))
                {
                    action.Invoke(prefab, taggable);
                }
            });
        }

        public static GameObject[] GetTaggedPrefabs(string[] withTags, string[] butNotWithTags, GameObject[] prefabs)
        {
            var result = new List<GameObject>();

            DoForEachTaggedPrefab(withTags, butNotWithTags, prefabs, (prefab, taggable) => {
                if (taggable.ShouldUse)
                {
                    result.Add(prefab);
                }
            });

            return result.ToArray();
        }

#if UNITY_EDITOR

        //[ContextMenu("TransferOpinionsToTaggables")]
        //public void TransferOpinionsToTaggables()
        //{
        //    LoadUserDecisions();

        //    foreach (var name in AssetOpinions.Opinions.Keys)
        //    {
        //        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(name);
        //        if (prefab == null)
        //        {
        //            continue;
        //        }
        //        var taggable = prefab.GetComponent<Taggable>();
        //        if (taggable != null)
        //        {
        //            AssetOpinions.TransferOpinionToTaggable(taggable);
        //        }
        //    }
        //}

        [ContextMenu("Save User Decisions")]
        private void SaveUserDecisions()
        {           
            var path = Application.persistentDataPath + "/assetYesNo.json";
            Debug.Log(path);

            var serializer = new DataContractJsonSerializer(typeof(AssetOpinions));

            using (var file = File.CreateText(path))
            {
                file.Write(AssetOpinions.ToJSON<AssetOpinions>());
            }
        }

        

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
					taggable.TagFromPath(path.ToLowerInvariant());
				}
				
				taggable.EnsureTag("lowpoly");
				taggable.PrefabAssetPath = path;
					
                if (taggable.WeightedTags == null || taggable.WeightedTags.Length == 0)
                {
					Debug.Log(taggable.name);
                }
            }
			AssetDatabase.SaveAssets();
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
