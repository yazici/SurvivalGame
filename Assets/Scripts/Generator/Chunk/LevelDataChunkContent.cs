using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pamux.Lib.LevelData.Generator
{
    public class LevelDataChunkContent
    {
        public readonly IList<GameObject> GameObjects = new List<GameObject>();
        public Terrain Terrain;
        public TerrainData TerrainData;

        private LevelDataChunk levelDataChunk;

        private static LevelDataGeneratorSettings Settings { get { return LevelDataChunk.Settings; } }
        public LevelDataMaps LevelDataMaps { get { return levelDataChunk.LevelDataMaps; } }


        public LevelDataChunkContent(LevelDataChunk levelDataChunk)
        {
            this.levelDataChunk = levelDataChunk;
        }


        internal void CreateGameObjects()
        {
        }

        public void CreateTerrain()
        {
            TerrainData = new TerrainData
            {
                heightmapResolution = Settings.HeightMapResolution,
                alphamapResolution = Settings.AlphaMapResolution
            };
            TerrainData.SetHeights(0, 0, LevelDataMaps.HeightMap);
            ApplyTextures();

            TerrainData.size = new Vector3(Settings.Length, Settings.Height, Settings.Length);
            var newTerrainGameObject = Terrain.CreateTerrainGameObject(TerrainData);
            newTerrainGameObject.transform.position = new Vector3(levelDataChunk.X * Settings.Length, 0, levelDataChunk.Z * Settings.Length);

            Terrain = newTerrainGameObject.GetComponent<Terrain>();
            Terrain.heightmapPixelError = 8;
            Terrain.materialType = UnityEngine.Terrain.MaterialType.Custom;
            Terrain.materialTemplate = Settings.TerrainMaterial;
            Terrain.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            Terrain.Flush();
        }

        private void ApplyTextures()
        {
            TerrainData.splatPrototypes = LevelDataMaps.SplatPrototypes;
            TerrainData.RefreshPrototypes();

            for (var zRes = 0; zRes < TerrainData.alphamapHeight; zRes++)
            {
                for (var xRes = 0; xRes < TerrainData.alphamapWidth; xRes++)
                {
                    /*var normalizedX = (float)xRes / (terrainData.alphamapWidth - 1);
                    var normalizedZ = (float)zRes / (terrainData.alphamapHeight - 1);

                    var steepness = terrainData.GetSteepness(normalizedX, normalizedZ);
                    var steepnessNormalized = Mathf.Clamp(steepness / 1.5f, 0, 1f);

                    if (steepnessNormalized > 0.5)
                    {
                        splatMap[zRes, xRes, 0] = 0.0f;
                        splatMap[zRes, xRes, 1] = 1.0f;

                    }
                    else
                    {
                        splatMap[zRes, xRes, 0] = 1.0f;
                        splatMap[zRes, xRes, 1] = 0.0f;

                    }
                    //splatMap[zRes, xRes, 0] = 1f - steepnessNormalized;
                    //splatMap[zRes, xRes, 1] = steepnessNormalized;
                    */


                    if (LevelDataMaps.BiomeMap[zRes, xRes] == 0)
                    {
                        LevelDataMaps.SplatMap[zRes, xRes, 0] = 0.0f;
                        LevelDataMaps.SplatMap[zRes, xRes, 1] = 1.0f;
                    }
                    else
                    {
                        LevelDataMaps.SplatMap[zRes, xRes, 0] = 1.0f;
                        LevelDataMaps.SplatMap[zRes, xRes, 1] = 0.0f;
                    }
                }
            }

            TerrainData.SetAlphamaps(0, 0, LevelDataMaps.SplatMap);
        }
    }
}
