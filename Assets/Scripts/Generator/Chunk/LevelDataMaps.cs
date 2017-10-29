using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pamux.Lib.LevelData.Generator
{

    [Serializable]
    public class BiomeData
    {
        public string Name;
        public Texture2D texture;
    }

    public class LevelDataMaps
    {
        public static LevelDataGeneratorSettings S { get { return LevelDataGeneratorSettings.Instance; } }
        public static LevelDataGenerator G { get { return LevelDataGenerator.Instance; } }

        public readonly float[,] HeightMap;
        public readonly float[,] TemperatureMap;
        public readonly float[,] MoistureMap;
        public readonly int[,] BiomeMap;

        public readonly float[,,] SplatMap;


        public readonly SplatPrototype[] SplatPrototypes;

        private LevelDataMaps()
        {
            BiomeMap = new int[S.BiomeMapResolution, S.BiomeMapResolution];
            HeightMap = new float[S.HeightMapResolution, S.HeightMapResolution];
            TemperatureMap = new float[S.TemperatureMapResolution, S.TemperatureMapResolution];
            MoistureMap = new float[S.MoistureMapResolution, S.MoistureMapResolution];

            SplatPrototypes = new SplatPrototype[S.Biomes.Count()];

            for (int i = 0; i < S.Biomes.Count(); ++i)
            {
                SplatPrototypes[i] = new SplatPrototype
                {
                    texture = S.Biomes[i].texture
                };
            }

            SplatMap = new float[S.AlphaMapResolution, S.AlphaMapResolution, S.Biomes.Count()];
        }

        public static LevelDataMaps CreateAndGenerate(int x, int z)
        {
            var levelDataMaps = new LevelDataMaps();

            for (var zRes = 0; zRes < S.HeightMapResolution; zRes++)
            {
                for (var xRes = 0; xRes < S.HeightMapResolution; xRes++)
                {
                    var xCoordinate = x + (float)xRes / (S.HeightMapResolution - 1);
                    var zCoordinate = z + (float)zRes / (S.HeightMapResolution - 1);

                    var noise = G.NoiseMaker.GetElevationNoise(xCoordinate, zCoordinate);
                    levelDataMaps.HeightMap[zRes, xRes] = 0;// noise;

                    noise = G.NoiseMaker.GetBiomeNoise(xCoordinate, zCoordinate);
                    levelDataMaps.BiomeMap[zRes, xRes] = GetBiomeId(noise);

                    noise = G.NoiseMaker.GetTemperatureNoise(xCoordinate, zCoordinate);
                    levelDataMaps.TemperatureMap[zRes, xRes] = noise;

                    noise = G.NoiseMaker.GetMoistureNoise(xCoordinate, zCoordinate);
                    levelDataMaps.MoistureMap[zRes, xRes] = noise;
                }
            }

            return levelDataMaps;
        }        

        private static int GetBiomeId(float noise)
        {
            float rangeSize = 1f / S.Biomes.Count();
            float value = rangeSize;
            for (int i = 0; i < S.Biomes.Count(); ++i)
            {
                if (noise < value)
                {
                    return i;
                }
                value += rangeSize;
            }
            return S.Biomes.Count() - 1;
        }
    }
}
