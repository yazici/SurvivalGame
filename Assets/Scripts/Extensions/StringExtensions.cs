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
    }
}
