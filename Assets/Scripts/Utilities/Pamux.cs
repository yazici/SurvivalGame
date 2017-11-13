using Pamux.Lib.Enums;
using System;

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pamux.Lib.Utilities
{
    public static class Diagnostics
    {
        public static void Log(string message, LogLevels logLevel = LogLevels.Informational, IDictionary<string, string> properties = null)
        {
            Debug.Log(message);
        }

        public static void Log(Exception ex, string message, LogLevels logLevel = LogLevels.Informational, IDictionary<string, string> properties = null)
        {
        }

        public static void LogIfNull(object obj, string message)
        {
            if (obj == null)
            { 
                Debug.Log(message);
            }
        }
    }
}