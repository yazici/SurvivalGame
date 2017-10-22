using System.Collections;
using Pamux.Lib.LevelData.Generator;
using UnityEngine;
using UnityStandardAssets.Utility;
using System;

public class LevelDataGenerator : MonoBehaviour
{
    private const int Radius = 4;

    private string previousPlayerPositionChunkKey;

    public Transform Player;

    public LevelDataGeneratorSettings Settings;

    public int seed = 1;

    public INoiseMaker NoiseMaker;

    void Awake()
    {
        if (LevelDataChunk.LevelDataGenerator != null)
        {
            throw new Exception("LevelDataGenerator must be singleton");
        }

        LevelDataChunk.LevelDataGenerator = this;
        LevelDataMaps.LevelDataGenerator = this;

        if (Settings == null)
        {
            throw new Exception("Settings can't be null");
        }
        
        NoiseMaker = new NoiseMaker(seed);
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
            var chunkKey = LevelDataChunk.GetKey(Player.position);
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
    }

    private void Update()
    {
        ChunkCache.TidyUp();
        if (!Player.gameObject.activeSelf)
        {
            return;
        }

        var playerChunkPosition = LevelDataChunk.GetKey(Player.position);
        if (playerChunkPosition != previousPlayerPositionChunkKey)
        {
            ChunkCache.Update(Player.position, Radius);
            previousPlayerPositionChunkKey = playerChunkPosition;
        }
    }
}