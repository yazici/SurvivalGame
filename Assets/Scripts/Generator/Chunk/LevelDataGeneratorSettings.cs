using Pamux.GameModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Utilities;

namespace Pamux.Lib.LevelData.Generator
{
    public class LevelDataGeneratorSettings : Singleton<LevelDataGeneratorSettings>
    {
        public int HeightMapResolution = 129;

        public int TemperatureMapResolution = 129;

        public int MoistureMapResolution = 129;

        public int BiomeMapResolution = 129;

        public int AlphaMapResolution = 129;

        public int Length = 100;

        public int Height = 40;

        public int Seed { get; private set; }

        public BiomeData[] Biomes;
        public Material TerrainMaterial;

        public float DefaultGameObjectElevation = 1.0f;
		public float DefaultCloudElevation = 100.0f;

        public Vector3 GetVector3AtDefaultElevation(int x, int z)
        {
            return new Vector3(x * Length, DefaultGameObjectElevation, z * Length);
        }
		
		
		public Vector3 GetVector3AtCloudElevation(int x, int z)
        {
            return new Vector3(x * Length, DefaultCloudElevation, z * Length);
        }
    }
}