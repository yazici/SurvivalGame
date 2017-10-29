using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AClockworkBerry;
using Pamux.Utilities;

public class GameManager : Singleton<GameManager>
{
    void Start () {
        ScreenLogger.Instance.ShowLog = true;
    }
}
