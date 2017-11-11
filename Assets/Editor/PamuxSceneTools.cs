using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Pamux.Lib.GameObjects;
using Pamux.Lib.WorldGen;
using Pamux.Lib.Managers;

namespace Pamux.Lib.Editors
{
    public static class SceneTools
    {
        public static IEnumerable<GameObject> GetChildren(GameObject parent) {
            var children = new List<GameObject>();

            foreach (var xform in UnityEngine.Object.FindObjectsOfType<Transform>())
            {
                if (xform.parent == parent)
                {
                    children.Add(xform.gameObject);
                }
            }

            return children;
        }
        

        public static GameObject ComponentExistsInChildren<T>(GameObject parent)
            where T : MonoBehaviour
        {
            foreach (var child in GetChildren(parent))
            {
                var c = child.GetComponent<T>();
                if (c != null)
                {
                    return c.gameObject;
                }
            }
            return null;
        }

        static public GameObject EnsureManagerComponent<T>(GameObject parent)
            where T: MonoBehaviour
        {
            var gameObject = ComponentExistsInChildren<T>(parent);
            if (gameObject!= null)
            {
                return gameObject;
            }

            var go = new GameObject();
            go.name = typeof(T).Name;
            go.AddComponent<T>();

            if (parent != null)
            { 
                go.transform.parent = parent.transform;
            }
            return go;
        }

        [MenuItem("Pamux/Setup Managers", false, 0)]
        static public void SetupManagers()
        {
            var gameManagerContainer = EnsureManagerComponent<GameManager>(null);

            EnsureManagerComponent<WorldManager>(gameManagerContainer);
            EnsureManagerComponent<PlayerManager>(gameManagerContainer);
            EnsureManagerComponent<UtilitiesManager>(gameManagerContainer);
            EnsureManagerComponent<ArtsManager>(gameManagerContainer);
            EnsureManagerComponent<MonetizationManager>(gameManagerContainer);
        }
    }
}
