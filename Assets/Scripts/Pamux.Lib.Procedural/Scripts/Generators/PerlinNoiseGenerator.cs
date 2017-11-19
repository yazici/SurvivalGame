using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using Pamux.Lib.Enums;
using Pamux.Lib.Procedural.Data;

namespace Pamux.Lib.Procedural
{
    public class Noise
    {
        private NoiseSettings settings;
        private System.Random prng;
        private Vector2[] octaveOffsets;
        private Vector2[] sampleOctaveOffsets;
        private float[] scaledFrequencies;
        private float[] amplitudes;
        private float maxPossibleHeight = 0f;

        public Noise(NoiseSettings settings)
        {
            this.settings = settings;
            prng = new System.Random(settings.seed);

            octaveOffsets = new Vector2[settings.octaves];
            sampleOctaveOffsets = new Vector2[settings.octaves];
            scaledFrequencies = new float[settings.octaves];
            amplitudes = new float[settings.octaves];

            InitializeOctaveValues();
        }

        private void InitializeOctaveValues()
        {
            maxPossibleHeight = 0f;
            var amplitude = 1f;
            var frequency = 1f;

            for (var i = 0; i < settings.octaves; ++i)
            {
                var offsetX = prng.Next(-100000, 100000) + settings.offset.x;
                var offsetY = prng.Next(-100000, 100000) - settings.offset.y;

                octaveOffsets[i] = new Vector2(offsetX, offsetY);
                scaledFrequencies[i] = settings.scale * frequency;

                amplitudes[i] = amplitude;

                maxPossibleHeight += amplitude;
                amplitude *= settings.persistence;
                frequency *= settings.lacunarity;
            }
        }

        public Task<float[,]> GenerateNoiseMap(int mapWidth, int mapHeight, Vector2 sampleCentre)
        {
            var noiseMap = new float[mapWidth, mapHeight];

            var halfWidth = mapWidth / 2f;
            var halfHeight = mapHeight / 2f;
            
            for (var i = 0; i < settings.octaves; ++i)
            {
                var sampleOffsetX = -halfWidth + octaveOffsets[i].x + sampleCentre.x;
                var sampleOffsetY = -halfHeight + octaveOffsets[i].y - sampleCentre.y;

                sampleOctaveOffsets[i] = new Vector2(sampleOffsetX, sampleOffsetY);
            }

            var maxLocalNoiseHeight = float.MinValue;
            var minLocalNoiseHeight = float.MaxValue;

            for (var y = 0; y < mapHeight; ++y)
            {
                for (var x = 0; x < mapWidth; ++x)
                {
                    var noiseHeight = 0f;

                    for (var i = 0; i < settings.octaves; ++i)
                    {
                        var sampleX = (x + sampleOctaveOffsets[i].x) / scaledFrequencies[i];
                        var sampleY = (y + sampleOctaveOffsets[i].y) / scaledFrequencies[i];

                        noiseHeight += settings.GetAmplifiedPerlinNoise(sampleX, sampleY, amplitudes[i]);
                    }

                    if (noiseHeight > maxLocalNoiseHeight)
                    {
                        maxLocalNoiseHeight = noiseHeight;
                    }
                    if (noiseHeight < minLocalNoiseHeight)
                    {
                        minLocalNoiseHeight = noiseHeight;
                    }
                    noiseMap[x, y] = noiseHeight;

                    if (settings.NormalizeGlobally)
                    {
                        var normalizedHeight = (noiseMap[x, y] + 1) / (maxPossibleHeight / 0.9f);
                        noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                    }
                }
            }

            if (settings.NormalizeLocally)
            {
                for (var y = 0; y < mapHeight; ++y)
                {
                    for (var x = 0; x < mapWidth; ++x)
                    {
                        noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                    }
                }
            }
            return Task.FromResult(noiseMap);
        }
    }
}