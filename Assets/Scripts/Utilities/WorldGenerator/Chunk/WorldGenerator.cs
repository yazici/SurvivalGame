using System.Collections;
using Pamux.Lib.WorldGen;
using UnityEngine;
using UnityStandardAssets.Utility;
using System;
using Pamux.Utilities;
using Pamux.Lib.Managers;

namespace Pamux.Lib.WorldGen
{
    public class WorldGenerator : Singleton<WorldGenerator>
    {
        public System.Random Random { get; private set; }

        private const int Radius = 4;

        private string previousPlayerPositionChunkKey;

        protected Transform Player => PlayerManager.Instance.LocalPlayer.transform;

        public WorldGeneratorSettings Settings;

        public INoiseMaker NoiseMaker;

        void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("WorldGenerator must be singleton");
            }

            Instance = this;

            if (Settings == null)
            {
                throw new Exception("Settings can't be null");
            }

            NoiseMaker = new NoiseMaker(Settings.Seed);

            Random = new System.Random(Settings.Seed);
        }

        void Start()
        {
            ChunkCache.Update(Player.position, Radius);
            StartCoroutine(InitializeCoroutine());
        }

        private IEnumerator InitializeCoroutine()
        {
            do
            {
                var chunkKey = WorldDataChunk.GetKey(Player.position);
                if (ChunkCache.IsChunkGenerated(chunkKey))
                {
                    previousPlayerPositionChunkKey = chunkKey;
                    ActivatePlayer();
                    break;
                }
                yield return null;
            } while (true);

        }

        private void ActivatePlayer()
        {

            Player.position = new Vector3(Player.position.x, ChunkCache.GetElevation(Player.position) + 0.5f, Player.position.z);
            Player.gameObject.SetActive(true);
            Player.GetComponent<Rigidbody>().useGravity = true;
        }

        private void Update()
        {
            ChunkCache.TidyUp();
            if (!Player.gameObject.activeSelf)
            {
                return;
            }

            var playerChunkPosition = WorldDataChunk.GetKey(Player.position);
            if (playerChunkPosition != previousPlayerPositionChunkKey)
            {
                ChunkCache.Update(Player.position, Radius);
                previousPlayerPositionChunkKey = playerChunkPosition;
            }
        }
    }
}