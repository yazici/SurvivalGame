using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Lib.Procedural.Data;

namespace Pamux.Lib.Procedural
{
    public class HeightMapGenerator
    {
        public static HeightMapGenerator Instance = null;

        private Noise noise;
        private HeightMapSettings settings;
        private AnimationCurve heightCurve_threadsafe;

        public HeightMapGenerator(HeightMapSettings settings)
        {
            if (Instance != null)
            {
                throw new System.Exception($"HeightMapGenerator is a singleton.");
            }

            Instance = this;

            this.settings = settings;
            noise = new Noise(settings.noiseSettings);
            heightCurve_threadsafe = new AnimationCurve(settings.heightCurve.keys);
        }


        private Task<HeightMap> ApplyHeightCurve(float[,] values)
        {
            var width = values.GetLength(0);
            var height = values.GetLength(1);

            var minValue = float.MaxValue;
            var maxValue = float.MinValue;

            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    values[x, y] *= heightCurve_threadsafe.Evaluate(values[x, y]) * settings.heightMultiplier;

                    if (values[x, y] > maxValue)
                    {
                        maxValue = values[x, y];
                    }
                    if (values[x, y] < minValue)
                    {
                        minValue = values[x, y];
                    }
                }
            }

            return Task.FromResult(new HeightMap(values, minValue, maxValue));
        }

        public async Task<HeightMap> GenerateHeightMap(int width, int height, Vector2 sampleCentre)
        {
            var heightMap = await noise.GenerateNoiseMap(width, height, sampleCentre);
            return await ApplyHeightCurve(heightMap);
        }
    }
}