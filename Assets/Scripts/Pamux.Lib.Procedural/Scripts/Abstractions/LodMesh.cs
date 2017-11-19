using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Lib.Procedural.Data;

namespace Pamux.Lib.Procedural.Abstractions
{
    public class LodMesh
    {

        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;

        public event System.Action<Vector2> updateCallback;

        public LodMesh(int lod)
        {
            this.lod = lod;
        }

        void OnMeshDataReceived(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;

            // TODO:
            var viewerPosition = Vector2.zero;
            updateCallback(viewerPosition);
        }

        //public void RequestMesh(HeightMap heightMap)
        //{
        //    hasRequestedMesh = true;

        //    MeshGenerator.Instance.GenerateTerrainMesh(heightMap.values, lod)
        //        .ContinueWith((meshData) => OnMeshDataReceived);
        //}

    }
}