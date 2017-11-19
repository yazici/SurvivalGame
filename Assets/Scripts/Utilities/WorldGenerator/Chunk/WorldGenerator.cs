using System.Collections;
using Pamux.Lib.WorldGen;
using UnityEngine;
using UnityStandardAssets.Utility;
using System;
using Pamux.Lib.Utilities;
using Pamux.Lib.Managers;

namespace Pamux.Lib.WorldGen
{
    [RequireComponent(typeof(WorldGeneratorSettings))]
    public class WorldGenerator : Singleton<WorldGenerator>
    {
        public static WorldGeneratorSettings S { get { return WorldGeneratorSettings.Instance; } }
        public static WorldGenerator G { get { return WorldGenerator.Instance; } }

        public System.Random Random { get; private set; }

        private const int Radius = 2;

        private string previousPositionChunkKey;

        private Vector3 Position => 
            PlayerManager.LocalPlayerPointOfView == null
                ? Vector3.zero
                : PlayerManager.LocalPlayerPointOfView.transform.position;

        public INoiseMaker NoiseMaker;

        void Start()
        {
            NoiseMaker = new NoiseMaker(S.Seed);
            Random = new System.Random(S.Seed);
            
            ChunkCache.Update(Position, Radius);
            StartCoroutine(InitializeCoroutine());
        }

        private IEnumerator InitializeCoroutine()
        {
            do
            {
                var chunkKey = WorldDataChunk.GetKey(Position);
                if (ChunkCache.IsChunkGenerated(chunkKey))
                {
                    previousPositionChunkKey = chunkKey;
                    //WorldManager.Instance.ChunkIsReady = true; IsReady
                    break;
                }
                yield return null;
            } while (true);

        }

        private void Update()
        {
            ChunkCache.TidyUp();
            var currentPositionChunkKey = WorldDataChunk.GetKey(Position);
            if (currentPositionChunkKey != previousPositionChunkKey)
            {
                ChunkCache.Update(Position, Radius);
                previousPositionChunkKey = currentPositionChunkKey;
            }
        }
    }
}