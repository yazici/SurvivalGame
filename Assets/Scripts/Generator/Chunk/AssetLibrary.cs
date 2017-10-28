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
        public GameObject[] trees;
        public GameObject[] houses;
        public GameObject[] flowers;
        public GameObject[] humans;
        public GameObject[] animals;
        public GameObject[] rocks;
        public GameObject[] clouds;
        public GameObject[] enemies;

#if UNITY_EDITOR
        private GameObject[] GetPrefabs(string[] searchInFolders)
        {
            var prefabs = new List<GameObject>();

            var guids = AssetDatabase.FindAssets(@"", searchInFolders);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                prefabs.Add(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            }

            return prefabs.ToArray();
        }


        [ContextMenu("Attach Prefabs")]
        private void AttachPrefabs()
        {
            rocks = GetPrefabs(new string[] { "Assets/Prefabs/Rocks" });
            clouds = GetPrefabs(new string[] { "Assets/Prefabs/Clouds" });
            enemies = GetPrefabs(new string[] { "Assets/Prefabs/Enemies" });
            flowers = GetPrefabs(new string[] { "Assets/Prefabs/Flowers" });
            houses = GetPrefabs(new string[] { "Assets/Prefabs/Buildings" });
            trees = GetPrefabs(new string[] { "Assets/Prefabs/Trees" });
            animals = GetPrefabs(new string[] { "Assets/Prefabs/Animals" });
            humans = GetPrefabs(new string[] { "Assets/Prefabs/People/CharactersNotAnimated" });
        }
#endif
    }
}
