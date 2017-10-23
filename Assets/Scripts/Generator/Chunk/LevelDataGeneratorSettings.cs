using Assets.Scripts.GameObjects;
using System;
using UnityEngine;

namespace Pamux.Lib.LevelData.Generator
{
    public class LevelDataGeneratorSettings : MonoBehaviour
    {
        public static LevelDataGeneratorSettings Instance { get; private set; }

        public int HeightMapResolution = 129;

        public int TemperatureMapResolution = 129;

        public int MoistureMapResolution = 129;

        public int BiomeMapResolution = 129;

        public int AlphaMapResolution = 129;

        public int Length = 100;

        public int Height = 40;

        public int Seed { get; private set; }

        public Texture2D FlatTexture;

        public Texture2D SteepTexture;

        public Material TerrainMaterial;

        public float DefaultGameObjectElevation = 1.0f;

        public GameObject[] trees;

        public GameObject[] houses;


        void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("LevelDataGeneratorSettings must be singleton");
            }

            Instance = this;
        }

        public Vector3 GetVector3AtDefaultElevation(int x, int z)
        {
            return new Vector3(x * Length, DefaultGameObjectElevation, z * Length);
        }
    }
}