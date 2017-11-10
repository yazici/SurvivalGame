using Assets.Scripts.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Generator
{
    [DataContract]
    public class AssetOpinions
    {
        [DataMember]
        public IDictionary<string, string> Opinions = new Dictionary<string, string>();

        private static string GetGameObjectPath(Transform transform)
        {
            var prefab = PrefabUtility.FindPrefabRoot(transform.gameObject);
            string path = prefab.name.Replace("(Clone)", "");
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.name + "/" + path;
            }
            return path;
        }

        internal void Set(Taggable taggable)
        {
            Debug.Log(taggable.PrefabAssetPath);
            var prefab = AssetDatabase.LoadAssetAtPath<Taggable>(taggable.PrefabAssetPath);
            prefab.Opinion = taggable.Opinion;
            //var path = taggable.name.Replace("(Clone)", "");
            Opinions[taggable.PrefabAssetPath] = taggable.Opinion;
        }

        internal void TransferOpinionToTaggable(Taggable taggable)
        {
            if (Opinions.ContainsKey(taggable.name))
            {
                taggable.Opinion = Opinions[taggable.name];
            }
        }
    }

    
}
