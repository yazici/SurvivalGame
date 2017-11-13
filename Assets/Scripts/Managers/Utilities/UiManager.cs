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
        public RectTransform CharacterTargetIndicator { get; private set; }
        public RectTransform ScreenSpaceCanvas { get; private set; }
        public RectTransform DebugUiPanel { get; private set; }

        public RectTransform MiniMapPanel { get; private set; }

        public RectTransform UiCamera { get; private set; }
        protected override void Awake()
        {
            base.Awake();


            UiCamera = transform.InstantiatePrefabAsChild("Prefabs/ui/UiCamera") as RectTransform;

            ScreenSpaceCanvas = transform.InstantiatePrefabAsChild("Prefabs/ui/ScreenSpaceCanvas") as RectTransform;
            var canvas = ScreenSpaceCanvas.GetComponent<Canvas>();
            canvas.worldCamera = UiCamera.GetComponent<Camera>();

            WorldSpaceCanvas = transform.InstantiatePrefabAsChild("Prefabs/ui/WorldSpaceCanvas") as RectTransform;

            //MiniMapPanel = ScreenSpaceCanvas.InstantiatePrefabAsChild("Prefabs/ui/MiniMapPanel") as RectTransform;
            //MiniMapPanel.offsetMax = Vector2.one;
            //MiniMapPanel.offsetMin = Vector2.zero;

            //DebugUiPanel = ScreenSpaceCanvas.InstantiatePrefabAsChild("Prefabs/ui/DebugUiPanel") as RectTransform;
            //DebugUiPanel.offsetMax = Vector2.one;
            //DebugUiPanel.offsetMin = Vector2.zero;

            CharacterTargetIndicator = WorldSpaceCanvas.InstantiatePrefabAsChild("Prefabs/ui/CharacterTargetIndicator") as RectTransform;
        }
    }
}