using LibNoise;
using LibNoise.Generator;
using UnityEngine;

namespace Pamux.Lib.LevelData.Generator
{
    public class NoiseMaker : INoiseMaker
    {
        private int seed;

        private int elevationSeed;
        private int temperatureSeed;
        private int moistureSeed;
        private int biomeSeed;

        private readonly ModuleBase elevationNoiseMaker = new Perlin();
        private readonly ModuleBase temperatureNoiseMaker = new Perlin();
        private readonly ModuleBase moistureNoiseMaker = new Perlin();
        private readonly ModuleBase biomeNoiseMaker = new Perlin();

        public NoiseMaker(int seed)
        {
            this.seed = seed;

            var random = new System.Random(seed);

            elevationSeed = seed + random.Next();
            temperatureSeed = seed + random.Next();
            moistureSeed = seed + random.Next();
            biomeSeed = seed + random.Next();
        }

        public float GetElevationNoise(float x, float z)
        {
            return (float)(elevationNoiseMaker.GetValue(x + elevationSeed, 0, z + elevationSeed) / 2f) + 0.5f;
        }

        public float GetTemperatureNoise(float x, float z)
        {
            return (float)(temperatureNoiseMaker.GetValue(x + temperatureSeed, 0, z + temperatureSeed) / 2f) + 0.5f;
        }

        public float GetMoistureNoise(float x, float z)
        {
            return (float)(moistureNoiseMaker.GetValue(x + moistureSeed, 0, z + moistureSeed) / 2f) + 0.5f;
        }

        public float GetBiomeNoise(float x, float z)
        {
            return (float)(biomeNoiseMaker.GetValue(x + biomeSeed, 0, z + biomeSeed) / 2f) + 0.5f;
        }
    }
}