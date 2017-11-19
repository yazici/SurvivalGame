using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pamux.Lib.Procedural.Interfaces;
using Pamux.Lib.Procedural.Abstractions;
using Pamux.Lib.Utilities;
using Pamux.Lib.Procedural.Data;

using UnityEngine;

namespace Pamux.Lib.Procedural
{
    public class TerrainChunkProvider : ITerrainChunkProvider
    {
        private LruCache<string, TerrainChunk> chunkCache = new LruCache<string, TerrainChunk>(32);

        public TerrainChunkProvider(TerrainSettings terrainSettings)
        {
            if (TerrainGenerator.Instance == null)
            {
                new TerrainGenerator(terrainSettings);
            }
        }

        public async Task<TerrainChunk> GetChunkAsync(Transform parent, int x, int y, int lodIndex)
        {
            var cacheKey = $"{x}:{y}:{lodIndex}";

            var chunk = chunkCache.Get(cacheKey);
            if (chunk == null)
            {
                chunk = await TerrainGenerator.Instance.GetChunkAsync(parent, x, y, lodIndex);
                chunkCache.Set(cacheKey, chunk);
            }
            
            return chunk;
        }
    }        
}
