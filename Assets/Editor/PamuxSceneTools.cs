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
using Pamux.Lib.Extensions;
using Pamux.Lib.Utilities;
using UnityEngine.UI;

namespace Pamux.Lib.Editors
{
    public static class SceneTools
    {
        //public static IEnumerable<GameObject> GetChildren(GameObject parent) {
        //    var children = new List<GameObject>();

        //    foreach (var xform in UnityEngine.Object.FindObjectsOfType<Transform>())
        //    {
        //        if (xform.parent == parent)
        //        {
        //            children.Add(xform.gameObject);
        //        }
        //    }

        //    return children;
        //}
        

        //public static GameObject ComponentExistsInChildren<T>(GameObject parent)
        //    where T : MonoBehaviour
        //{
        //    foreach (var child in GetChildren(parent))
        //    {
        //        var c = child.GetComponent<T>();
        //        if (c != null)
        //        {
        //            return c.gameObject;
        //        }
        //    }
        //    return null;
        //}

        //static public GameObject EnsureChildWithComponent<T>(GameObject parent)
        //    where T: MonoBehaviour
        //{
        //    var gameObject = ComponentExistsInChildren<T>(parent);
        //    if (gameObject!= null)
        //    {
        //        return gameObject;
        //    }

        //    var go = new GameObject();
        //    go.name = typeof(T).Name;
        //    go.AddComponent<T>();

        //    if (parent != null)
        //    { 
        //        go.transform.parent = parent.transform;
        //    }
        //    return go;
        //}

        [MenuItem("Pamux/Setup Managers", false, 0)]
        static public void SetupManagers()
        {
            MonoBehaviour root = null;

            var gameManager = root.EnsureChildWithComponent<GameManager>();
            var configurationManager = gameManager.EnsureChildWithComponent<ConfigurationManager>();

            var worldManager = gameManager.EnsureChildWithComponent<WorldManager>();
            var timeManager = worldManager.EnsureChildWithComponent<TimeManager>();
            var weatherManager = worldManager.EnsureChildWithComponent<WeatherManager>();

            var utilitiesManager = gameManager.EnsureChildWithComponent<UtilitiesManager>();

            var uiManager = utilitiesManager.EnsureChildWithComponent<UiManager>();

            var transform = uiManager.transform.InstantiatePrefabAsChild("Prefabs/ui/ScreenSpaceCanvas");
            var rectTransform = transform.InstantiatePrefabAsChild("Prefabs/ui/DebugUiPanel") as RectTransform;
            rectTransform.offsetMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;

            transform = uiManager.transform.InstantiatePrefabAsChild("Prefabs/ui/WorldSpaceCanvas");
            transform.InstantiatePrefabAsChild("Prefabs/ui/ShowMouseClick");

            var artsManager = gameManager.EnsureChildWithComponent<ArtsManager>();
            var cinematographyManager = artsManager.EnsureChildWithComponent<CinematographyManager>();
            var photographyManager = artsManager.EnsureChildWithComponent<PhotographyManager>();


            var monetizationManager = gameManager.EnsureChildWithComponent<MonetizationManager>();


            var playerManager = gameManager.EnsureChildWithComponent<PlayerManager>();
        }
    }
}
