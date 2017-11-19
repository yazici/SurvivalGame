using UnityEngine;
using System.Collections;
using Pamux.Lib.Procedural.Data;
using Pamux.Lib.Procedural.Enums;

namespace Pamux.Lib.Procedural
{
    public class MapPreview : MonoBehaviour
    {
        public Renderer textureRender;
        public MeshFilter meshFilter;
        public MeshRenderer meshRenderer;

        public DrawModes drawMode;

        private MeshSettings meshSettings => terrainSettings.meshSettings;
        private HeightMapSettings heightMapSettings => terrainSettings.heightMapSettings;
        private TextureData textureData => terrainSettings.textureSettings;

        public TerrainSettings terrainSettings;

        public Material terrainMaterial;

        [Range(0, LodInfo.numSupportedLODs - 1)]
        public int editorPreviewLOD;
        public bool autoUpdate;

        public void DrawMapInEditor()
        {
            if (VertexInfo.TerrainSettings == null)
            {
                VertexInfo.TerrainSettings = terrainSettings;
            }

            if (drawMode == DrawModes.FalloffMap)
            {
                var falloffMap = FalloffGenerator.GenerateFalloffMap(meshSettings.numVertsPerLine);
                var texture = TextureGenerator.TextureFromHeightMap(new HeightMap(falloffMap, 0, 1));
                DrawTexture(texture);
                return;
            }

            
            if (HeightMapGenerator.Instance == null)
            {
                new HeightMapGenerator(heightMapSettings);
            }

            var heightMap = HeightMapGenerator.Instance.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, Vector2.zero).GetAwaiter().GetResult();
            if (drawMode == DrawModes.NoiseMap)
            {
                var texture = TextureGenerator.TextureFromHeightMap(heightMap);
                DrawTexture(texture);
                return;
            }

            if (MeshGenerator.Instance == null)
            {
                new MeshGenerator(meshSettings);
            }

            var meshData = MeshGenerator.Instance.GenerateTerrainMesh(heightMap.values, editorPreviewLOD);
            DrawMesh(meshData);
        }

        public void DrawTexture(Texture2D texture)
        {
            textureRender.sharedMaterial.mainTexture = texture;
            textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height) / 10f;

            textureRender.gameObject.SetActive(true);
            meshFilter.gameObject.SetActive(false);
        }

        public void DrawMesh(MeshData meshData)
        {
            meshFilter.sharedMesh = meshData.CreateMesh();

            textureRender.gameObject.SetActive(false);
            meshFilter.gameObject.SetActive(true);
        }



        void OnValuesUpdated()
        {
            if (!Application.isPlaying)
            {
                DrawMapInEditor();
            }
        }

        void OnTextureValuesUpdated()
        {
            textureData.ApplyToMaterial(terrainMaterial);
        }

        void OnValidate()
        {
            if (terrainSettings == null)
            {
                return;
            }

            if (meshSettings != null)
            {
                meshSettings.OnValuesUpdated -= OnValuesUpdated;
                meshSettings.OnValuesUpdated += OnValuesUpdated;
            }
            if (heightMapSettings != null)
            {
                heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
                heightMapSettings.OnValuesUpdated += OnValuesUpdated;
            }
            if (textureData != null)
            {
                textureData.OnValuesUpdated -= OnTextureValuesUpdated;
                textureData.OnValuesUpdated += OnTextureValuesUpdated;
            }

        }

    }
}