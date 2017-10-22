using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pamux.Lib.LevelData.Generator
{
    public class LevelDataMaps
    {
        public static LevelDataGenerator LevelDataGenerator;

        public readonly float[,] HeightMap;
        public readonly float[,] TemperatureMap;
        public readonly float[,] MoistureMap;
        public readonly int[,] BiomeMap;

        private const int SplatTextureCount = 2;
        public readonly float[,,] SplatMap;


        public readonly SplatPrototype[] SplatPrototypes;

        private LevelDataMaps()
        {
            BiomeMap = new int[LevelDataGenerator.Settings.BiomeMapResolution, LevelDataGenerator.Settings.BiomeMapResolution];
            HeightMap = new float[LevelDataGenerator.Settings.HeightMapResolution, LevelDataGenerator.Settings.HeightMapResolution];
            TemperatureMap = new float[LevelDataGenerator.Settings.TemperatureMapResolution, LevelDataGenerator.Settings.TemperatureMapResolution];
            MoistureMap = new float[LevelDataGenerator.Settings.MoistureMapResolution, LevelDataGenerator.Settings.MoistureMapResolution];

            SplatPrototypes = new SplatPrototype[]
            {
                new SplatPrototype
                {
                    texture = LevelDataGenerator.Settings.FlatTexture
                },
                new SplatPrototype
                {
                    texture = LevelDataGenerator.Settings.SteepTexture
                }
            };

            SplatMap = new float[LevelDataGenerator.Settings.AlphaMapResolution, LevelDataGenerator.Settings.AlphaMapResolution, SplatPrototypes.Count()];
        }

        public static LevelDataMaps CreateAndGenerate(int x, int z)
        {
            var levelDataMaps = new LevelDataMaps();

            for (var zRes = 0; zRes < LevelDataGenerator.Settings.HeightMapResolution; zRes++)
            {
                for (var xRes = 0; xRes < LevelDataGenerator.Settings.HeightMapResolution; xRes++)
                {
                    var xCoordinate = x + (float)xRes / (LevelDataGenerator.Settings.HeightMapResolution - 1);
                    var zCoordinate = z + (float)zRes / (LevelDataGenerator.Settings.HeightMapResolution - 1);

                    var noise = LevelDataGenerator.NoiseMaker.GetElevationNoise(xCoordinate, zCoordinate);
                    levelDataMaps.HeightMap[zRes, xRes] = noise;

                    noise = LevelDataGenerator.NoiseMaker.GetBiomeNoise(xCoordinate, zCoordinate);
                    if (noise > 0.5)
                    {
                        levelDataMaps.BiomeMap[zRes, xRes] = 0;
                    }
                    else
                    {
                        levelDataMaps.BiomeMap[zRes, xRes] = 1;
                    }

                    noise = LevelDataGenerator.NoiseMaker.GetTemperatureNoise(xCoordinate, zCoordinate);
                    levelDataMaps.TemperatureMap[zRes, xRes] = noise;

                    noise = LevelDataGenerator.NoiseMaker.GetMoistureNoise(xCoordinate, zCoordinate);
                    levelDataMaps.MoistureMap[zRes, xRes] = noise;
                }
            }

            return levelDataMaps;
        }
    }
}
