using System;
using Pamux.Lib.Extensions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pamux.Lib.Enums;
using Pamux.Lib.Managers;

namespace Pamux.Lib.Utilities
{
    public class DebugUi : Singleton<DebugUi>
    {
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

        private void Start()
        {
            transform.AddChildButtonOnClickListener("ButtonScreenLogToggle", () => { IsExpanded = !IsExpanded; });
            transform.AddChildButtonOnClickListener("ButtonZoomIn", () => { ZoomLevel += ZoomLevelDelta;  });
            transform.AddChildButtonOnClickListener("ButtonZoomOut", () => { ZoomLevel -= ZoomLevelDelta; });
            transform.AddChildButtonOnClickListener("ButtonTogglePointOfView", () => {
                PlayerManager.LocalPlayerPointOfViewType = PlayerManager.LocalPlayerPointOfViewType == PlayerPointOfViewTypes.ThirdPerson 
                    ? PlayerPointOfViewTypes.FirstPerson
                    : PlayerPointOfViewTypes.ThirdPerson;
            });
        }
    }
}
