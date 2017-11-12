using Pamux.Lib.Enums;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;

namespace Pamux.Lib.Extensions
{
    public static class StringExtensions
    {
        private static IDictionary<PamuxResourceTypes, string> ResourceTypeToPathPrefix = new Dictionary<PamuxResourceTypes, string>()
        {
            { PamuxResourceTypes.BiomeGroundMaterial, "Materials/Ground" },
            { PamuxResourceTypes.BiomeGroundTexture, "Textures/Ground" },
            { PamuxResourceTypes.CharacterModels, "Models/Characters" }
        };

        public static T LoadResource<T>(this string resourceName, PamuxResourceTypes resourceType)
            where T : UnityEngine.Object
        {
            return Resources.Load($"{ResourceTypeToPathPrefix[resourceType]}/{resourceName}") as T;
        }

        public static string ToHex(this Color32 color)
        {
            return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
        }

        public static Color ToColor(this string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = hex.Length >= 8 
                    ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)
                    : (byte) 255;

            return new Color32(r, g, b, a);
        }

    }
}
