using UnityEngine;
using Pamux.Lib.Utilities;
using Pamux.Lib.WorldGen;
using System;
using Pamux.Lib.Procedural.Data;
using System.Threading;
using Pamux.Lib.Procedural.Interfaces;
using Pamux.Lib.Procedural;
using Pamux.Lib.Procedural.Abstractions;
using System.Threading.Tasks;

namespace Pamux.Lib.Managers
{
    //[RequireComponent(typeof(WorldGenerator))]
    [RequireComponent(typeof(TerrainSettings))]
    //[RequireComponent(typeof(TimeManager))]
    //[RequireComponent(typeof(WeatherManager))]

    // [RequireComponent(typeof(TerrainManager))]
    // [RequireComponent(typeof(TerrainObjectsManager))]    
    // [RequireComponent(typeof(WorldDiscoveryManager))]


    public class WorldManager : Singleton<WorldManager>
    {
        public static ManualResetEventSlim IsReadyEvent = new ManualResetEventSlim(false);

        private ITerrainChunkProvider terrainChunkProvider;
        public ITerrainChunkProvider TerrainChunkProvider => terrainChunkProvider;

        private TerrainSettings terrainSettings;

        protected override void Awake()
        {
            base.Awake();

            terrainSettings = GetComponent<TerrainSettings>();
            terrainChunkProvider = new TerrainChunkProvider(terrainSettings);

            IsReadyEvent.Set();
        }

        public Vector3 GetGroundLevelPosition(Vector3 position)
        {
            return new Vector3(position.x, 100.0f, position.z);
        }

        internal Task<TerrainChunk> GetChunkAsync(int x, int y, int lodIndex)
        {
            return terrainChunkProvider.GetChunkAsync(transform, x, y, lodIndex);
        }
    }
}