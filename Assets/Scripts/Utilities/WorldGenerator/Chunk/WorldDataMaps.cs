using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pamux.Lib.WorldGen
{

    

    public class WorldDataMaps
    {
        public static WorldGeneratorSettings S { get { return WorldGeneratorSettings.Instance; } }
        public static WorldGenerator G { get { return WorldGenerator.Instance; } }

        public readonly WorldDataMap HeightMap;
        public readonly WorldDataMap TemperatureMap;
        public readonly WorldDataMap MoistureMap;
        public readonly WorldDataMap BiomeMap;

        public readonly SplatPrototype[] SplatPrototypes;
        public readonly float[,,] SplatMap;

        private WorldDataMaps()
        {
            HeightMap = new WorldDataMap(S.HeightMapResolution);
            TemperatureMap = new WorldDataMap(S.TemperatureMapResolution);
            MoistureMap = new WorldDataMap(S.MoistureMapResolution);
            BiomeMap = new WorldDataMap(S.BiomeMapResolution);

            SplatMap = new float[S.AlphaMapResolution, S.AlphaMapResolution, S.Biomes.Count()];
            SplatPrototypes = new SplatPrototype[S.Biomes.Count()];

            for (int i = 0; i < S.Biomes.Count(); ++i)
            {
                SplatPrototypes[i] = new SplatPrototype
                {
                    texture = S.Biomes[i].Texture
                };
            }
        }

        public static WorldDataMaps CreateAndGenerate(int cX, int cZ)
        {
            var worldDataMaps = new WorldDataMaps();

            worldDataMaps.HeightMap.ForEachPoint(cX, cZ, (x, z) => G.NoiseMaker.GetElevationNoise(x, z));
            worldDataMaps.TemperatureMap.ForEachPoint(cX, cZ, (x, z) => G.NoiseMaker.GetTemperatureNoise(x, z));
            worldDataMaps.MoistureMap.ForEachPoint(cX, cZ, (x, z) => G.NoiseMaker.GetMoistureNoise(x, z));
            worldDataMaps.BiomeMap.ForEachPoint(cX, cZ, (x, z) => G.NoiseMaker.GetBiomeNoise(x, z));

            return worldDataMaps;
        }        

        public int GetBiomeId(int x, int z)
        {
            return GetBiomeId(BiomeMap.Map[x, z]);
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
