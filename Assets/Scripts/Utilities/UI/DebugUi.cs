using System;
using Pamux.Lib.Extensions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pamux.Lib.Enums;
using Pamux.Lib.Managers;
using UnityEngine;

namespace Pamux.Lib.Utilities
{
    public class DebugUi : Singleton<DebugUi>
    {
        private RectTransform DebugUiPanel;

        public bool isExpanded;
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            private set
            {
                if (value == isExpanded)
                {
                    return;
                }
                isExpanded = value;

                if (isExpanded)
                {
                    DebugUiPanel.offsetMax = Vector2.one *3;
                    DebugUiPanel.offsetMin = Vector2.zero;

                }
                else
                { 
                    DebugUiPanel.offsetMax = Vector2.one;
                    DebugUiPanel.offsetMin = Vector2.zero;
                }
                
            }
        }

        public const float ZoomLevelDelta = 1f;
        private float zoomLevel;
        public float ZoomLevel
        {
            get
            {
                return zoomLevel;
            }
            private set
            {
                if (value == zoomLevel)
                {
                    return;
                }
                zoomLevel = value;
            }
        }



        private void ZoomBy(float sign)
        {
            if (PlayerManager.LocalPlayerPointOfViewType != PlayerPointOfViewTypes.ThirdPerson)
            {
                return;
            }
            ZoomLevel += sign * ZoomLevelDelta;
        }

        protected override void Awake()
        {
            base.Awake();

            DebugUiPanel = transform as RectTransform;
        }

        private void Start()
        {
            transform.AddChildButtonOnClickListener("ButtonScreenLogToggle", () => { IsExpanded = !IsExpanded; });
            transform.AddChildButtonOnClickListener("ButtonZoomIn", () => { ZoomBy(-1); });
            transform.AddChildButtonOnClickListener("ButtonZoomOut", () => { ZoomBy(1); });
            transform.AddChildButtonOnClickListener("ButtonTogglePointOfView", () => {
                PlayerManager.LocalPlayerPointOfViewType = PlayerManager.LocalPlayerPointOfViewType == PlayerPointOfViewTypes.ThirdPerson 
                    ? PlayerPointOfViewTypes.FirstPerson
                    : PlayerPointOfViewTypes.ThirdPerson;
            });
        }
    }
}
