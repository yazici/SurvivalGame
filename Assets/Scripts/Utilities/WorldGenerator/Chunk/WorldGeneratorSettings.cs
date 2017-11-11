using Pamux.GameModel;
using System;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Utilities;

namespace Pamux.Lib.WorldGen
{

    public class WorldGeneratorSettings : Singleton<WorldGeneratorSettings>
    {

        public float Oceans = 0.75f;
        public float Land = 0.24f;
        public float Lakes = 0.02f;
        public float Rivers = 0.01f;

        public WorldSizes WorldSize;

        public int HeightMapResolution = 129;

        public int TemperatureMapResolution = 129;

        public int MoistureMapResolution = 129;

        public int BiomeMapResolution = 129;

        public int AlphaMapResolution = 129;

        public int Length = 100;

        public int Height = 40;

        //http://www.jgallant.com/procedurally-generating-wrapping-world-maps-in-unity-csharp-part-4/
        // https://github.com/jongallant/WorldGeneratorFinal
        
        public AnimationCurve TemperatureCurve;
        public AnimationCurve MoistureCurve;
        public AnimationCurve PrecipitationCurve;
        public AnimationCurve LandmassCurve;
        public AnimationCurve CrimeCurve;
        public AnimationCurve RockDensityCurve;
        public AnimationCurve GoldDensityCurve;
        public AnimationCurve IronDensityCurve;
        public AnimationCurve OceanSalinityCurve;
        public AnimationCurve WindSpeedCurve;
        public AnimationCurve RiverDensityCurve;
        public AnimationCurve LakeDensityCurve;
        public AnimationCurve MicroClimateProbabilityCurve;

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