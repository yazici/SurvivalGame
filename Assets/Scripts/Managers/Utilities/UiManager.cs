using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Lib.Utilities;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pamux.Lib.Extensions;

using D  = Pamux.Lib.Utilities.Diagnostics;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(EventSystem))]
    [RequireComponent(typeof(StandaloneInputModule))]    

    public class UiManager : Singleton<UiManager>
    {
        public RectTransform WorldSpaceCanvas { get; private set; }
        public RectTransform CharacterTargetIndicator { get; private set; }
        public RectTransform ScreenSpaceOrthographicCanvas { get; private set; }
        public RectTransform ScreenSpacePerspectiveCanvas { get; private set; }
        public RectTransform DebugUiPanel { get; private set; }

        public RectTransform MiniMapPanel { get; private set; }

        public Transform UiCamera { get; private set; }
        protected override void Awake()
        {
            base.Awake();


            ScreenSpacePerspectiveCanvas = transform.InstantiatePrefabAsChild("Prefabs/ui/ScreenSpacePerspectiveCanvas") as RectTransform;
            ScreenSpaceOrthographicCanvas = transform.InstantiatePrefabAsChild("Prefabs/ui/ScreenSpaceOrthographicCanvas") as RectTransform;
            WorldSpaceCanvas = transform.InstantiatePrefabAsChild("Prefabs/ui/WorldSpaceCanvas") as RectTransform;


            CharacterTargetIndicator = WorldSpaceCanvas.InstantiatePrefabAsChild("Prefabs/ui/CharacterTargetIndicator") as RectTransform;
        }
    }
}