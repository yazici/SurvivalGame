using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Lib.Enums;
using Pamux.Lib.Procedural.Enums;

namespace Pamux.Lib.Procedural.Data
{
    [System.Serializable]
    public class NoiseSettings
    {
        public NormalizeMode normalizeMode;

        public bool NormalizeGlobally => normalizeMode == NormalizeMode.Global;
        public bool NormalizeLocally => normalizeMode == NormalizeMode.Local;

        public float scale = 50;

        public int octaves = 6;
        [Range(0, 1)]
        public float persistence = .6f;
        public float lacunarity = 2;

        public int seed;
        public Vector2 offset;

        public float GetAmplifiedPerlinNoise(float x, float y, float amplitude)
        {
            // TODO: Use noiseMaker
            return amplitude * (Mathf.PerlinNoise(x, y) * 2 - 1);
        }

        public void ValidateValues()
        {
            scale = Mathf.Max(scale, 0.01f);
            octaves = Mathf.Max(octaves, 1);
            lacunarity = Mathf.Max(lacunarity, 1);
            persistence = Mathf.Clamp01(persistence);
        }
    }
}