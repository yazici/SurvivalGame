using System;
using UnityEngine;

namespace Pamux.Lib.LevelData.Generator
{
    public class LevelDataGeneratorSettings : MonoBehaviour
    {
        public int HeightMapResolution = 129;

        public int TemperatureMapResolution = 129;

        public int MoistureMapResolution = 129;

        public int BiomeMapResolution = 129;

        public int AlphaMapResolution = 129;

        public int Length = 100;

        public int Height = 40;

        public Texture2D FlatTexture;

        public Texture2D SteepTexture;

        public Material TerrainMaterial;
    }
}