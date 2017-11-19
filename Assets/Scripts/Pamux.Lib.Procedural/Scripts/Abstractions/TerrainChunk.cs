using UnityEngine;
using Pamux.Lib.Procedural.Data;

namespace Pamux.Lib.Procedural.Abstractions
{
    public class TerrainChunk
    {

        private const float colliderGenerationDistanceThreshold = 5;

        public event System.Action<TerrainChunk, bool> onVisibilityChanged;
        public Coord coord;
        public int lodIndex;

        private GameObject meshObject;
        private Vector2 sampleCentre;
        private Bounds bounds;

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;

        private LodInfo[] detailLevels => terrainSettings.detailLevels;
        private LodMesh[] lodMeshes;
        private int colliderLODIndex => terrainSettings.colliderLODIndex;

        private HeightMap heightMap;
        private bool heightMapReceived;
        private int previousLODIndex = -1;
        private bool hasSetCollider;
        private float maxViewDst;

        private TerrainSettings terrainSettings;
        private MeshSettings meshSettings => terrainSettings.meshSettings;

        public TerrainChunk(Transform parent, TerrainSettings terrainSettings, Coord coord, int lodIndex)
        {
            this.terrainSettings = terrainSettings;

            this.coord = coord;
            this.lodIndex = lodIndex;

            var position = new Vector2(coord.x * meshSettings.meshWorldSize, coord.y * meshSettings.meshWorldSize);
            sampleCentre = position / meshSettings.meshScale;

            bounds = new Bounds(position, Vector2.one * meshSettings.meshWorldSize);


            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshRenderer.material = terrainSettings.mapMaterial;

            meshObject.transform.position = new Vector3(position.x, 0, position.y);
            meshObject.transform.parent = parent;
            SetVisible(false);

            lodMeshes = new LodMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; ++i)
            {
                lodMeshes[i] = new LodMesh(detailLevels[i].lod);
                lodMeshes[i].updateCallback += UpdateTerrainChunk;
                if (i == colliderLODIndex)
                {
                    lodMeshes[i].updateCallback -= UpdateCollisionMesh;
                    lodMeshes[i].updateCallback += UpdateCollisionMesh;
                }
            }

            maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        }


        //public void Load()
        //{
        //    HeightMapGenerator.Instance.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, sampleCentre)
        //        .ContinueWith((heightMap)=> OnHeightMapReceived(heightMap));

        //    //ThreadedDataRequester.RequestData(() => generateHeightMap, OnHeightMapReceived);
        //}



        //void OnHeightMapReceived(HeightMap heightMap)
        //{
        //    this.heightMap = heightMap;
        //    heightMapReceived = true;

        //    // TODO:
        //    Vector2 viewerPosition = Vector2.zero;
        //    UpdateTerrainChunk(viewerPosition);
        //}

        public void UpdateTerrainChunk(Vector2 viewerPosition)
        {
            if (!heightMapReceived)
            {
                return;
            }

            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

            bool wasVisible = IsVisible();
            bool visible = viewerDstFromNearestEdge <= maxViewDst;

            if (visible)
            {
                int lodIndex = 0;

                for (int i = 0; i < detailLevels.Length - 1; ++i)
                {
                    if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
                    {
                        lodIndex = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (lodIndex != previousLODIndex)
                {
                    var lodMesh = lodMeshes[lodIndex];
                    if (lodMesh.hasMesh)
                    {
                        previousLODIndex = lodIndex;
                        meshFilter.mesh = lodMesh.mesh;
                    }
                    else if (!lodMesh.hasRequestedMesh)
                    {
                        //lodMesh.RequestMesh(heightMap);
                    }
                }


            }

            if (wasVisible != visible)
            {

                SetVisible(visible);
                if (onVisibilityChanged != null)
                {
                    onVisibilityChanged(this, visible);
                }
            }
        }

        public void UpdateCollisionMesh(Vector2 viewerPosition)
        {
            if (hasSetCollider)
            {
                return;
            }

            float sqrDstFromViewerToEdge = bounds.SqrDistance(viewerPosition);

            if (sqrDstFromViewerToEdge < detailLevels[colliderLODIndex].sqrVisibleDstThreshold)
            {
                if (!lodMeshes[colliderLODIndex].hasRequestedMesh)
                {
                    // TODO: lodMeshes[colliderLODIndex].RequestMesh(heightMap);
                }
            }

            if (sqrDstFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold)
            {
                if (lodMeshes[colliderLODIndex].hasMesh)
                {
                    meshCollider.sharedMesh = lodMeshes[colliderLODIndex].mesh;
                    hasSetCollider = true;
                }
            }
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }

    }

    
}