using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Pamux.Lib.LevelData.Generator
{
    public class LevelDataChunk
    {
        public int X { get; private set; }
        public int Z { get; private set; }

        public string Key
        {
            get
            {
                return GetKey(X, Z);
            }
        }

        private HashSet<LevelDataChunk> Neighbors = new HashSet<LevelDataChunk>();

        private object lockObject = new object();

        private readonly LevelDataChunkContent content;

        public LevelDataMaps LevelDataMaps { get; private set; }

        public static LevelDataGenerator LevelDataGenerator;
        public static LevelDataGeneratorSettings Settings { get { return LevelDataGenerator == null ? null :  LevelDataGenerator.Settings; } }
        

        internal static void WorldToChunkPosition(Vector3 worldPosition, ref int x, ref int z)
        {
            if (Settings == null)
            {
                throw new Exception("Settings is null");
            }

            x = (int)Mathf.Floor(worldPosition.x / Settings.Length);
            z = (int)Mathf.Floor(worldPosition.z / Settings.Length);
        }

        internal static string GetKey(int x, int z)
        {
            return string.Format("{0}:{1}", x, z);
        }

        internal static string GetKey(Vector3 position)
        {
            int x = 0;
            int z = 0;

            WorldToChunkPosition(position, ref x, ref z);

            return LevelDataChunk.GetKey(x, z);
        }

        public LevelDataChunk(int x, int z)
        {
            X = x;
            Z = z;

            content = new LevelDataChunkContent(this);
        }

        #region Heightmap stuff

        public void GenerateLevelDataMaps()
        {
            var thread = new Thread(GenerateLevelDataMapsThread);
            thread.Start();
        }

        
        private void GenerateLevelDataMapsThread()
        {
            var levelDataMaps = LevelDataMaps.CreateAndGenerate(X, Z);
            lock (lockObject)
            { 
                this.LevelDataMaps = levelDataMaps;
            }
        }

        public bool IsHeightMapReady()
        {
            lock (lockObject)
            {
                return content.Terrain == null && LevelDataMaps != null;
            }
        }

        public float GetElevation(Vector3 worldPosition)
        {
            return content.Terrain.SampleHeight(worldPosition);
        }

        #endregion

        public void CreateContent()
        {
            content.CreateTerrain();
            content.CreateGameObjects();
        }

        internal LevelDataChunk GetGeneratedNeighbor(int deltaX, int deltaZ)
        {
            return ChunkCache.GetGeneratedChunk(X + deltaX, Z + deltaZ);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Z.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as LevelDataChunk;
            if (other == null)
            { 
                return false;
            }

            return X == other.X && Z == other.Z;
        }

        #region Chunk removal

        public void Remove()
        {
            lock (lockObject)
            { 
                LevelDataMaps = null;
            }

            foreach (var neighbor in Neighbors)
            {
                neighbor.Neighbors.Remove(this);
            }
            Neighbors.Clear();

            if (content.Terrain != null)
            { 
                GameObject.Destroy(content.Terrain.gameObject);
            }
        }

        public void RemoveFromNeighborhood(LevelDataChunk chunk)
        {
            if (chunk != null)
            {
                chunk.Neighbors.Remove(this);
                Neighbors.Remove(chunk);
            }
        }

        #endregion

        #region Neighbors

        public void UpdateNeighbors()
        {
            if (content.Terrain != null)
            {
                content.Terrain.SetNeighbors(LeftNeighborTerrain, TopNeighborTerrain, RightNeighborTerrain, BottomNeighborTerrain);
                content.Terrain.Flush();
            }
        }

        private int LeftX
        {
            get
            {
                return X - 1;
            }
        }

        private int RightX
        {
            get
            {
                return X + 1;
            }
        }

        private int TopZ
        {
            get
            {
                return Z - 1;
            }
        }

        private int BottomZ
        {
            get
            {
                return Z + 1;
            }
        }

        private LevelDataChunk LeftNeighbor
        {
            get
            {
                return ChunkCache.GetGeneratedChunk(LeftX, Z);
            }
        }

        private LevelDataChunk RightNeighbor
        {
            get
            {
                return ChunkCache.GetGeneratedChunk(RightX, Z);
            }
        }
        private LevelDataChunk TopNeighbor
        {
            get
            {
                return ChunkCache.GetGeneratedChunk(X, TopZ);
            }
        }

        private LevelDataChunk BottomNeighbor
        {
            get
            {
                return ChunkCache.GetGeneratedChunk(X, BottomZ);
            }
        }

        private Terrain LeftNeighborTerrain
        {
            get
            {
                var dataChunk = LeftNeighbor;
                if (dataChunk == null)
                {
                    return null;
                }
                return dataChunk.content.Terrain;
            }
        }

        private Terrain TopNeighborTerrain
        {
            get
            {
                var dataChunk = TopNeighbor;
                if (dataChunk == null)
                {
                    return null;
                }
                return dataChunk.content.Terrain;
            }
        }

        private Terrain RightNeighborTerrain
        {
            get
            {
                var dataChunk = RightNeighbor;
                if (dataChunk == null)
                {
                    return null;
                }
                return dataChunk.content.Terrain;
            }
        }

        private Terrain BottomNeighborTerrain
        {
            get
            {
                var dataChunk = BottomNeighbor;
                if (dataChunk == null)
                {
                    return null;
                }
                return dataChunk.content.Terrain;
            }
        }

        internal void SetAllNeighbors()
        {            
            MakeNeighbors(this, ChunkCache.GetGeneratedChunk(LeftX, Z));
            MakeNeighbors(this, ChunkCache.GetGeneratedChunk(RightX, Z));
            MakeNeighbors(this, ChunkCache.GetGeneratedChunk(X, TopZ));
            MakeNeighbors(this, ChunkCache.GetGeneratedChunk(X, BottomZ));
        }

        private void MakeNeighbors(LevelDataChunk levelDataChunk1, LevelDataChunk levelDataChunk2)
        {
            if (levelDataChunk1 == null || levelDataChunk2 == null)
            {
                return;
            }

            levelDataChunk1.Neighbors.Add(levelDataChunk2);
            levelDataChunk2.Neighbors.Add(levelDataChunk1);
        }

        
        #endregion
    }
}