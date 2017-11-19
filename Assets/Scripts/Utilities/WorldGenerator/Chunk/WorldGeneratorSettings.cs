using UnityEngine;
using Pamux.Lib.Utilities;
using Pamux.Lib.Enums;
using Pamux.Lib.Extensions;
using Pamux.Lib.Procedural.Enums;

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

        public float ChunkWorldWidth = 1000.0f; // 1km - x
        public float ChunkWorldLength = 1000.0f;// 1km - z
        public float ChunkWorldDiagonal;
        public float ChunkWorldHalfDiagonal;

        public float MaxHeight = 40.0f; // 40m - y

        public Vector3 ChunkSize { get; private set; }

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

        protected override void Awake()
        {
            base.Awake();

            ChunkWorldDiagonal = Mathf.Sqrt(ChunkWorldLength * ChunkWorldLength + ChunkWorldWidth * ChunkWorldWidth);
            ChunkWorldHalfDiagonal = ChunkWorldDiagonal / 2;

            ChunkSize = new Vector3(ChunkWorldWidth, MaxHeight, ChunkWorldLength);

            if (Biomes == null || Biomes.Length == 0)
            { 
                Biomes = new BiomeData[]
                {
                    new BiomeData("Ice"),
                    new BiomeData("Grass"),
                    new BiomeData("Desert"),
                };
            }

            if (TerrainMaterial == null)
            { 
                TerrainMaterial = "Ground".LoadResource<Material>(PamuxResourceTypes.BiomeGroundMaterial);
            }
        }

        internal Vector2 ChunkCenterWorld(int x, int z)
        {
            return new Vector2(x * ChunkWorldWidth, z * ChunkWorldLength);
        }

        internal Vector3 ChunkCenterWorldAtZeroElevation(int x, int z)
        {
            return new Vector3(x * ChunkWorldWidth, 0.0f, z * ChunkWorldLength);
        }
        internal Vector3 ChunkCenterWorldAtDefaultElevation(int x, int z)
        {
            return new Vector3(x * ChunkWorldWidth, DefaultGameObjectElevation, z * ChunkWorldLength);
        }
        internal Vector3 ChunkCenterWorldAtCloudElevation(int x, int z)
        {
            return new Vector3(x * ChunkWorldWidth, DefaultCloudElevation, z * ChunkWorldLength);
        }
    }
}