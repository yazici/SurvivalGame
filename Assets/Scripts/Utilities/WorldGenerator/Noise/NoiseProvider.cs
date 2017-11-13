using LibNoise;
using LibNoise.Generator;
using UnityEngine;

namespace Pamux.Lib.WorldGen
{
    public class NoiseMaker : INoiseMaker
    {
        private int seed;

        private System.Random random;

        private double elevationSeed;
        private double temperatureSeed;
        private double moistureSeed;
        private double biomeSeed;

        private readonly ModuleBase elevationNoiseMaker = new Perlin();
        private readonly ModuleBase temperatureNoiseMaker = new Perlin();
        private readonly ModuleBase moistureNoiseMaker = new Perlin();
        private readonly ModuleBase biomeNoiseMaker = new Perlin();

        public NoiseMaker(int seed)
        {
            this.seed = seed;

            random = new System.Random(seed);

            elevationSeed =  random.NextDouble();
            temperatureSeed = random.NextDouble();
            moistureSeed = random.NextDouble();
            biomeSeed =  random.NextDouble();
        }

        public float GetElevationNoise(float x, float z)
        {
            return (float)(elevationNoiseMaker.GetValue(x, elevationSeed, z) / 2f) + 0.5f;
        }

        public float GetTemperatureNoise(float x, float z)
        {
            return (float)(temperatureNoiseMaker.GetValue(x, temperatureSeed, z) / 2f) + 0.5f;
        }

        public float GetMoistureNoise(float x, float z)
        {
            return (float)(moistureNoiseMaker.GetValue(x, moistureSeed, z) / 2f) + 0.5f;
        }

        public float GetBiomeNoise(float x, float z)
        {
            return (float)(biomeNoiseMaker.GetValue(x, biomeSeed, z) / 2f) + 0.5f;
        }
    }
}