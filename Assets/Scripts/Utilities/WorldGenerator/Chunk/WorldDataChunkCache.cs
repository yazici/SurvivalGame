﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pamux.Lib.WorldGen
{
    public static class ChunkCache
    {
        public static readonly IDictionary<string, WorldDataChunk> GeneratedChunks = new Dictionary<string, WorldDataChunk>();

        internal static WorldDataChunk GetGeneratedChunk(string key)
        {
            return GeneratedChunks.ContainsKey(key)
                ? GeneratedChunks[key]
                : null;
        }

        internal static WorldDataChunk GetGeneratedChunk(int x, int z)
        {
            return GetGeneratedChunk(WorldDataChunk.GetKey(x, z));
        }

        internal static WorldDataChunk GetGeneratedChunk(Vector3 worldPosition)
        {
            return GetGeneratedChunk(WorldDataChunk.GetKey(worldPosition));
        }

        public static bool IsChunkGenerated(string chunkKey)
        {
            return GeneratedChunks.ContainsKey(chunkKey);
        }

        internal static bool IsChunkGenerated(int x, int z)
        {
            return IsChunkGenerated(WorldDataChunk.GetKey(x, z));
        }

        internal static bool IsChunkGenerated(Vector3 worldPosition)
        {
            return IsChunkGenerated(WorldDataChunk.GetKey(worldPosition));
        }

        internal static IList<string> GetChunkKeysInRadius(string chunkKeyAtOrigin, int radius)
        {
            var chunkAtOrigin = ChunkCache.GetGeneratedChunk(chunkKeyAtOrigin);
            var result = new List<string>();

            for (var zCircle = -radius; zCircle <= radius; zCircle++)
            {
                for (var xCircle = -radius; xCircle <= radius; xCircle++)
                {
                    if (xCircle * xCircle + zCircle * zCircle < radius * radius)
                    {
                        result.Add(WorldDataChunk.GetKey(chunkAtOrigin.X + xCircle, chunkAtOrigin.Z + zCircle));
                    }
                }
            }

            return result;
        }

        internal static IList<string> GetChunkKeysInRadius(int x, int z, int radius)
        {
            return GetChunkKeysInRadius(WorldDataChunk.GetKey(x, z), radius);
        }

        internal static IList<string> GetChunkKeysInRadius(Vector3 worldPosition, int radius)
        {
            return GetChunkKeysInRadius(WorldDataChunk.GetKey(worldPosition), radius);
        }

        private const int MaxChunkThreads = 3;

        private static readonly IDictionary<string, WorldDataChunk> RequestedChunks = new Dictionary<string, WorldDataChunk>();

        private static readonly IDictionary<string, WorldDataChunk> ChunksBeingGenerated = new Dictionary<string, WorldDataChunk>();

        private static readonly HashSet<string> ChunksToRemove = new HashSet<string>();

        public static OnChunkGeneratedDelegate OnChunkGenerated { get; set; }

        public static void TidyUp()
        {
            CompleteDeferredRemoves();

            GenerateMapsForAvailableChunks();
            CreateContentForReadyChunks();
        }

        public static void AddNewChunk(WorldDataChunk chunk)
        {
            RequestedChunks.Add(chunk.Key, chunk);
            GenerateMapsForAvailableChunks();
        }

        public static void RemoveChunk(int x, int z)
        {
            ChunksToRemove.Add(WorldDataChunk.GetKey(x, z));
            CompleteDeferredRemoves();
        }

        public static bool ChunkCanBeAdded(int x, int z)
        {
            var key = WorldDataChunk.GetKey(x, z);
            return
                !(RequestedChunks.ContainsKey(key)
                || ChunksBeingGenerated.ContainsKey(key)
                || GeneratedChunks.ContainsKey(key));
        }

        public static bool ChunkCanBeRemoved(int x, int z)
        {
            var key = WorldDataChunk.GetKey(x, z);
            return
                RequestedChunks.ContainsKey(key)
                || ChunksBeingGenerated.ContainsKey(key)
                || GeneratedChunks.ContainsKey(key);
        }

        public static IList<string> GetGeneratedChunks()
        {
            return GeneratedChunks.Keys.ToList();
        }

        private static void GenerateMapsForAvailableChunks()
        {
            var requestedChunks = RequestedChunks.ToList();
            
            if (requestedChunks.Count > 0 && ChunksBeingGenerated.Count < MaxChunkThreads)
            {
                var chunksToAdd = requestedChunks.Take(MaxChunkThreads - ChunksBeingGenerated.Count);

                foreach (var chunkEntry in chunksToAdd)
                {
                    ChunksBeingGenerated.Add(chunkEntry.Key, chunkEntry.Value);

                    RequestedChunks.Remove(chunkEntry.Key);

                    chunkEntry.Value.GenerateWorldDataMaps();
                }
            }
        }


        public static float GetElevation(float x, float z)
        {
            return GetElevation(new Vector3(x, 0.0f, z));
        }

        public static float GetElevation(Vector3 worldPosition)
        {
            var chunk = GetGeneratedChunk(worldPosition);
            if (chunk != null)
            {
                return chunk.GetElevation(worldPosition);
            }

            return 0;
        }

        public static void Update(Vector3 worldPosition, int radius)
        {
            
            var chunkAtOrigin = ChunkCache.GetGeneratedChunk(worldPosition);

            int chunkAtOriginX;
            int chunkAtOriginZ;
            if (chunkAtOrigin == null)
            {
                chunkAtOriginX = 0;
                chunkAtOriginZ = 0;
            }
            else
            {
                chunkAtOriginX = chunkAtOrigin.X;
                chunkAtOriginZ = chunkAtOrigin.Z;
            }

            var rSquared = radius * radius;

            for (var deltaZ = -radius; deltaZ <= radius; ++deltaZ)
            {
                var zSquared = rSquared - (deltaZ * deltaZ);

                for (var deltaX = -radius; deltaX <= radius; ++deltaX)
                {
                    var chunkX = chunkAtOriginX + deltaX;
                    var chunkZ = chunkAtOriginZ + deltaZ;
                    var existingChunk = ChunkCache.GetGeneratedChunk(chunkX, chunkZ); 
                    var isInsideRadius = deltaX * deltaX < zSquared;

                    if (isInsideRadius)
                    {
                        if (existingChunk == null)
                        {
                            if (ChunkCanBeAdded(chunkX, chunkZ))
                            {
                                AddNewChunk(new WorldDataChunk(chunkX, chunkZ));
                            }
                        }
                        else
                        {
                            // already there... nothing to do
                        }
                    }
                    else
                    {                        
                        if (existingChunk == null)
                        {
                            // not there anyways... nothing to do
                        }
                        else
                        {
                            if (ChunkCanBeRemoved(chunkX, chunkZ))
                            {
                                RemoveChunk(chunkX, chunkZ);
                            }
                        }
                    }
                }
            }
        }

        private static void CreateContentForReadyChunks()
        {
            var neighborUpdateRequired = false;

            var chunks = ChunksBeingGenerated.ToList();
            foreach (var chunk in chunks)
            {
                if (chunk.Value.IsHeightMapReady())
                {
                    ChunksBeingGenerated.Remove(chunk.Key);
                    GeneratedChunks.Add(chunk.Key, chunk.Value);

                    chunk.Value.CreateContent();

                    neighborUpdateRequired = true;
                    if (OnChunkGenerated != null)
                        OnChunkGenerated.Invoke(ChunksBeingGenerated.Count);

                    chunk.Value.SetAllNeighbors();
                }
            }

            if (neighborUpdateRequired)
            { 
                UpdateAllChunkNeighbors();
            }
        }

        

        private static void CompleteDeferredRemoves()
        {
            var chunksToRemove = ChunksToRemove.ToList();

            foreach (var chunkPosition in chunksToRemove)
            {
                if (RequestedChunks.ContainsKey(chunkPosition))
                {
                    RequestedChunks.Remove(chunkPosition);
                    ChunksToRemove.Remove(chunkPosition);
                    continue;
                }

                if (GeneratedChunks.ContainsKey(chunkPosition))
                {
                    var chunk = GeneratedChunks[chunkPosition];
                    chunk.Remove();

                    GeneratedChunks.Remove(chunkPosition);
                    ChunksToRemove.Remove(chunkPosition);
                    continue;
                }

                if (!ChunksBeingGenerated.ContainsKey(chunkPosition))
                { 
                    ChunksToRemove.Remove(chunkPosition);
                }
            }
        }

        private static void UpdateAllChunkNeighbors()
        {
            foreach (var chunkEntry in GeneratedChunks)
            { 
                chunkEntry.Value.UpdateNeighbors();
            }
        }

    }
}