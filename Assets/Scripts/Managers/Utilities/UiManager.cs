using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Lib.Utilities;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pamux.Lib.Extensions;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(EventSystem))]
    [RequireComponent(typeof(StandaloneInputModule))]    

    public class UiManager : Singleton<UiManager>
    {
        public RectTransform WorldSpaceCanvas { get; private set; }
        public RectTransform ShowMouseClick { get; private set; }
        public RectTransform ScreenSpaceCanvas { get; private set; }
        public RectTransform DebugUiPanel { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            ScreenSpaceCanvas = transform.InstantiatePrefabAsChild("Prefabs/ui/ScreenSpaceCanvas") as RectTransform;

            DebugUiPanel = ScreenSpaceCanvas.InstantiatePrefabAsChild("Prefabs/ui/DebugUiPanel") as RectTransform;
            DebugUiPanel.offsetMax = Vector2.one;
            DebugUiPanel.offsetMin = Vector2.zero;


            WorldSpaceCanvas = transform.InstantiatePrefabAsChild("Prefabs/ui/WorldSpaceCanvas") as RectTransform;
            ShowMouseClick = WorldSpaceCanvas.InstantiatePrefabAsChild("Prefabs/ui/ShowMouseClick") as RectTransform;
        }
    }
}