﻿using UnityEngine;
using System.Collections;

namespace Pamux.Lib.Utilities
{
    public static class StringBuilding
    {
        public static string GetColorisedPlayerName(string name, Color color)
        {
            return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), name);
        }

        
    }
}