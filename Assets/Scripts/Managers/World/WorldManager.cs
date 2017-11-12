using UnityEngine;
using Pamux.Lib.Utilities;
using Pamux.Lib.WorldGen;
using System;

namespace Pamux.Lib.Managers
{
    [RequireComponent(typeof(WorldGenerator))]
    //[RequireComponent(typeof(TimeManager))]
    //[RequireComponent(typeof(WeatherManager))]

    // [RequireComponent(typeof(TerrainManager))]
    // [RequireComponent(typeof(TerrainObjectsManager))]    
    // [RequireComponent(typeof(WorldDiscoveryManager))]


    public class WorldManager : Singleton<WorldManager>
    {
        public bool ChunkIsReady { get; set; }

        protected override void Awake()
        {
            base.Awake();

            ChunkIsReady = false;
        }
    }
}