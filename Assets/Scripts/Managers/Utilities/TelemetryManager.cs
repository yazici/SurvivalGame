using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AClockworkBerry;
using Pamux.Lib.Utilities;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(ScreenLogger))]
    public class TelemetryManager : Singleton<TelemetryManager>
    {
        protected override void Awake()
        {
            base.Awake();
            ScreenLogger.Instance.ShowLog = false;
        }

    }
}