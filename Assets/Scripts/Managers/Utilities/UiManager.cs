using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Lib.Utilities;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(EventSystem))]
    [RequireComponent(typeof(StandaloneInputModule))]

    

    public class UiManager : Singleton<UiManager>
    {
        public UiCanvas InstantiatePrefabAsChild(string path)
        {
            var res = Resources.Load(path);
            var go = Instantiate(res) as GameObject;
            go.transform.parent = this.transform;
            return go.GetComponent<UiCanvas>();
        }

        protected override void Awake()
        {
        }
    }
}