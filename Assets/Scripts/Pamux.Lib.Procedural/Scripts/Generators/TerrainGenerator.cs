using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pamux.Lib.Procedural.Data;
using System.Collections.Concurrent;
using Pamux.Lib.Procedural.Abstractions;
using Pamux.Lib.Procedural.Interfaces;

namespace Pamux.Lib.Procedural
{
    public class TerrainGenerator : ITerrainChunkProvider
    {
        public static TerrainGenerator Instance = null;

        private TerrainSettings terrainSettings;

        private MeshSettings meshSettings => terrainSettings.meshSettings;
        private HeightMapSettings heightMapSettings => terrainSettings.heightMapSettings;
        private TextureData textureSettings => terrainSettings.textureSettings;

        private LodInfo[] detailLevels => terrainSettings.detailLevels;


        public TerrainGenerator(TerrainSettings terrainSettings)
        {
            if (Instance != null)
            {
                throw new System.Exception($"TerrainGenerator is a singleton.");
            }

            Instance = this;

            this.terrainSettings = terrainSettings;
            VertexInfo.TerrainSettings = terrainSettings;

            new HeightMapGenerator(heightMapSettings);
            new MeshGenerator(meshSettings);
        }

        public Task<TerrainChunk> GetChunkAsync(Transform parent, int x, int y, int lodIndex)
        {
            return Task.FromResult(new TerrainChunk(parent, 
                terrainSettings,
                new Coord(x, y),
                lodIndex));
        }
    }
}