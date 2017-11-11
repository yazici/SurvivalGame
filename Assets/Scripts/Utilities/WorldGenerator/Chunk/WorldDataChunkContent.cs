using Assets.Scripts.GameObjects;
using Assets.Scripts.Generator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using UnityEngine;

namespace Pamux.Lib.WorldGen
{
    public class WorldDataChunkContent
    {
        public static AssetLibrary A { get { return AssetLibrary.Instance; } }
        public static WorldGeneratorSettings S { get { return WorldGeneratorSettings.Instance; } }
        public static WorldGenerator G { get { return WorldGenerator.Instance; } }

        public readonly IList<GameObject> GameObjects = new List<GameObject>();
        public Terrain Terrain;
        public TerrainData TerrainData;

        private WorldDataChunk WorldDataChunk;
        private GameObjectFactory gameObjectFactory;

        public WorldDataMaps DataMaps { get { return WorldDataChunk.DataMaps; } }


        public WorldDataChunkContent(WorldDataChunk WorldDataChunk)
        {
            this.WorldDataChunk = WorldDataChunk;
            gameObjectFactory = new GameObjectFactory(this);
        }

        internal void CreateTestObjects()
        {
            var chunkOrigin = WorldDataChunk.OriginAtDefaultElevation;
            var o = gameObjectFactory.Create(GameObjectTypes.PrimitiveSphere, chunkOrigin);

            o.transform.localScale *= 2f;

            foreach (var chunkCorner in WorldDataChunk.CornersAtDefaultElevation)
            {
                o = gameObjectFactory.Create(GameObjectTypes.PrimitiveCylinder, chunkCorner);

                o.transform.localScale *= 2f;
            }
        }
        
        internal void CreateRandomObjects(ObjectCreationParameters parameters, Func<Vector3> randomVector3Provider = null)
        {
			if (parameters.Templates == null)
            { 
                throw new ArgumentNullException(nameof(parameters.Templates));
			}

            var chunkOrigin = WorldDataChunk.OriginAtDefaultElevation;
			
			if (randomVector3Provider == null)
			{
				randomVector3Provider = () => WorldDataChunk.NextRandomVector3OnSurface;
			}

            for (int i = 0; i < parameters.Count; ++i)
            {
                gameObjectFactory.CreateRandom(parameters, randomVector3Provider.Invoke());
            }
        }
        
        internal void CreateGameObjects()
        {
            CreateRandomObjects(new ObjectCreationParameters { Templates = A.SnowyTrees, Count = 30, MinHeight = 4.0f, MaxHeight = 8.0f });
            CreateRandomObjects(new ObjectCreationParameters { Templates = A.GreenFire, Count = 1, MinHeight = 1.0f, MaxHeight = 1.0f });
            CreateRandomObjects(new ObjectCreationParameters { Templates = A.Fire, Count = 1, MinHeight = 1.0f, MaxHeight = 1.0f });

            CreateRandomObjects(new ObjectCreationParameters { Templates = A.Flowers, Count = 15, MinHeight = 0.1f, MaxHeight = 0.3f });
            CreateRandomObjects(new ObjectCreationParameters { Templates = A.Animals, Count = 1, MinHeight = 1f, MaxHeight = 1f });
            CreateRandomObjects(new ObjectCreationParameters { Templates = A.People, Count = 2, MinHeight = 1f, MaxHeight = 1f });
            CreateRandomObjects(new ObjectCreationParameters { Templates = A.Cactus, Count = 1, MinHeight = 1f, MaxHeight = 3f });

            CreateRandomObjects(new ObjectCreationParameters { Templates = A.Rocks, Count = 2, MinHeight = 1f, MaxHeight = 3f });
            CreateRandomObjects(new ObjectCreationParameters { Templates = A.Houses, Count = 2, MinHeight = 6f, MaxHeight = 12f });


            //CreateRandomObjects(10, A.LandAnimals);

            //CreateRandomObjects(6, A.animals);

            //CreateRandomObjects(5, A.clouds, () => WorldDataChunk.NextRandomVector3OnTheClouds);
        }

        private void CreateRandomObjects(int v, object flowers)
        {
            throw new NotImplementedException();
        }

        public void CreateTerrain()
        {
            TerrainData = new TerrainData
            {
                heightmapResolution = S.HeightMapResolution,
                alphamapResolution = S.AlphaMapResolution
            };
            TerrainData.SetHeights(0, 0, DataMaps.HeightMap.Map);
            ApplyTextures();

            TerrainData.size = new Vector3(S.Length, S.Height, S.Length);
            var newTerrainGameObject = Terrain.CreateTerrainGameObject(TerrainData);
            
            newTerrainGameObject.transform.position = new Vector3(WorldDataChunk.X * S.Length, 0, WorldDataChunk.Z * S.Length);

            Terrain = newTerrainGameObject.GetComponent<Terrain>();
            Terrain.heightmapPixelError = 8;
            Terrain.materialType = UnityEngine.Terrain.MaterialType.Custom;
            Terrain.materialTemplate = S.TerrainMaterial;
            Terrain.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            Terrain.Flush();
        }

        

        private void ApplyTextures()
        {
            TerrainData.splatPrototypes = DataMaps.SplatPrototypes;
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
                        splatMap[xRes, zRes, 0] = 0.0f;
                        splatMap[xRes, zRes, 1] = 1.0f;

                    }
                    else
                    {
                        splatMap[xRes, zRes, 0] = 1.0f;
                        splatMap[xRes, zRes, 1] = 0.0f;

                    }
                    //splatMap[xRes, zRes, 0] = 1f - steepnessNormalized;
                    //splatMap[xRes, zRes, 1] = steepnessNormalized;
                    */

                    for (int i = 0; i < S.Biomes.Count(); ++i)
                    { 
                        DataMaps.SplatMap[xRes, zRes, i] = 0.0f;
                    }

                    var biomeId = DataMaps.GetBiomeId(xRes, zRes);

                    DataMaps.SplatMap[xRes, zRes, biomeId] = 1.0f;
                }
            }

            TerrainData.SetAlphamaps(0, 0, DataMaps.SplatMap);
        }
    }
}
