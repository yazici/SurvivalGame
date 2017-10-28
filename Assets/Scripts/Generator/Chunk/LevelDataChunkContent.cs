using Assets.Scripts.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Pamux.Lib.LevelData.Generator
{
    public class LevelDataChunkContent
    {
        public static AssetLibrary A { get { return AssetLibrary.Instance; } }
        public static LevelDataGeneratorSettings S { get { return LevelDataGeneratorSettings.Instance; } }
        public static LevelDataGenerator G { get { return LevelDataGenerator.Instance; } }

        public readonly IList<GameObject> GameObjects = new List<GameObject>();
        public Terrain Terrain;
        public TerrainData TerrainData;

        private LevelDataChunk levelDataChunk;
        private GameObjectFactory gameObjectFactory;

        public LevelDataMaps LevelDataMaps { get { return levelDataChunk.LevelDataMaps; } }


        public LevelDataChunkContent(LevelDataChunk levelDataChunk)
        {
            this.levelDataChunk = levelDataChunk;
            gameObjectFactory = new GameObjectFactory(this);
        }

        internal void CreateTestObjects()
        {
            var chunkOrigin = levelDataChunk.OriginAtDefaultElevation;
            var o = gameObjectFactory.Create(GameObjectTypes.PrimitiveSphere, chunkOrigin);

            o.transform.localScale *= 2f;

            foreach (var chunkCorner in levelDataChunk.CornersAtDefaultElevation)
            {
                o = gameObjectFactory.Create(GameObjectTypes.PrimitiveCylinder, chunkCorner);

                o.transform.localScale *= 2f;
            }
        }
        
        internal void CreateRandomObjects(int count, GameObject[] gameObjectTemplates, Func<Vector3> randomVector3Provider = null)
        {
			if (gameObjectTemplates == null)
			{
					throw new ArgumentNullException(nameof(gameObjectTemplates));
			}
            var chunkOrigin = levelDataChunk.OriginAtDefaultElevation;
			
			if (randomVector3Provider == null)
			{
				randomVector3Provider = () => levelDataChunk.NextRandomVector3OnSurface;
			}

            for (int i = 0; i < count; ++i)
            {
                gameObjectFactory.CreateRandom(gameObjectTemplates, randomVector3Provider.Invoke());
            }
        }

        internal void CreateGameObjects()
        {
            CreateRandomObjects(10, A.houses);
            CreateRandomObjects(100, A.trees);
            CreateRandomObjects(100, A.flowers);
            CreateRandomObjects(10, A.humans);
            CreateRandomObjects(6, A.animals);
            CreateRandomObjects(10, A.rocks);
            CreateRandomObjects(5, A.clouds, () => levelDataChunk.NextRandomVector3OnTheClouds);
        }

        public void CreateTerrain()
        {
            TerrainData = new TerrainData
            {
                heightmapResolution = S.HeightMapResolution,
                alphamapResolution = S.AlphaMapResolution
            };
            TerrainData.SetHeights(0, 0, LevelDataMaps.HeightMap);
            ApplyTextures();

            TerrainData.size = new Vector3(S.Length, S.Height, S.Length);
            var newTerrainGameObject = Terrain.CreateTerrainGameObject(TerrainData);
            newTerrainGameObject.transform.position = new Vector3(levelDataChunk.X * S.Length, 0, levelDataChunk.Z * S.Length);

            Terrain = newTerrainGameObject.GetComponent<Terrain>();
            Terrain.heightmapPixelError = 8;
            Terrain.materialType = UnityEngine.Terrain.MaterialType.Custom;
            Terrain.materialTemplate = S.TerrainMaterial;
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
