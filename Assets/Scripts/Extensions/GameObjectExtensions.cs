using System.Collections.Generic;
using UnityEngine;

namespace Pamux.Lib.Extensions
{
	public static class GameObjectExtensions
	{
		/// <summary>
		/// Sets the layer for this game object and all its children
		/// </summary>
		public static void SetLayerRecursively(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;
			SetLayerForChildren(gameObject.transform, layer);
		}

		private static void SetLayerForChildren(Transform transform, int layer)
		{
			int numChildren = transform.childCount;
			if (numChildren > 0)
			{
				for (int i = 0; i < numChildren; ++i)
				{
					Transform child = transform.GetChild(i);
					child.gameObject.layer = layer;
					SetLayerForChildren(child, layer);
				}
			}
		}


        public static T EnsureChildWithComponent<T>(this MonoBehaviour monoBehaviour)
            where T : MonoBehaviour
        {
            T component;
            if (monoBehaviour != null)
            {
                component = monoBehaviour.GetComponentInChildren<T>();
                if (component != null)
                {
                    return component;
                }
            }

            var go = new GameObject();
            go.name = typeof(T).Name;
            component = go.AddComponent<T>();

            if (monoBehaviour != null)
            {
                go.transform.SetParent(monoBehaviour.transform);
            }
            return component;
        }

        public static Transform InstantiatePrefabAsChild(this Transform transform, string path)
        {
            var res = Resources.Load(path);
            var go = GameObject.Instantiate(res) as GameObject;
            go.transform.SetParent(transform);
            return go.transform;
        }

        public static void DisableComponentInChildren<T>(this MonoBehaviour monoBehaviour)
            where T : MonoBehaviour
        {
            var c = monoBehaviour.GetComponentInChildren<T>();
            if (c != null)
            {
                c.enabled = false;
            }
        }

        public static void EnableComponentInChildren<T>(this MonoBehaviour monoBehaviour)
            where T : MonoBehaviour
        {
            var c = monoBehaviour.GetComponentInChildren<T>();
            if (c != null)
            {
                c.enabled = true;
            }
        }
    }
}