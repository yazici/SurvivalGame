using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Lib.Utilities;

namespace Pamux.Lib.Procedural.Data
{
    public class TerrainSettings : Singleton<TerrainSettings>
    {
        public int colliderLODIndex;
        public LodInfo[] detailLevels;

        public MeshSettings meshSettings;
        public HeightMapSettings heightMapSettings;
        public TextureData textureSettings;

        public Material mapMaterial;

        void Start()
        {
            textureSettings.ApplyToMaterial(mapMaterial);
            textureSettings.UpdateMeshHeights(mapMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);
        }
    }
}