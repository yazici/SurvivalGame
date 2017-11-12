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
    }
}