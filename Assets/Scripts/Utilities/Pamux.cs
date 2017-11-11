using Pamux.Lib.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pamux.Lib.Utilities
{
    public static class Pamux
    {
        public static void Log(string message, LogLevels logLevel = LogLevels.Informational, IDictionary<string, string> properties = null)
        {
        }

        public static void Log(Exception ex, string message, LogLevels logLevel = LogLevels.Informational, IDictionary<string, string> properties = null)
        {
        }
    }
}