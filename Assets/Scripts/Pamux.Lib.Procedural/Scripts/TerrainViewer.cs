using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Lib.Procedural.Interfaces;
using Pamux.Lib.Procedural.Abstractions;
using Pamux.Lib.Procedural.Data;
using Pamux.Lib.Utilities;

namespace Pamux.Lib.Procedural
{

    public class TerrainViewer : Singleton<TerrainViewer>
    {
        private const float moveThresholdForChunkUpdate = 25f;
        private const float moveThresholdForChunkUpdateSquared = moveThresholdForChunkUpdate * moveThresholdForChunkUpdate;

        private ITerrainChunkProvider TerrainChunkProvider;

        private Vector2 currentPosition;
        private Vector2 chunkVisibilityEvaluationPosition;

        // TODO:???
        private IList<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();
        private IDictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();


        private bool IsOnChunkVisibilityEvaluationPosition => currentPosition == chunkVisibilityEvaluationPosition;
        private bool ShouldReevaluateChunkVisibility => (chunkVisibilityEvaluationPosition - currentPosition).sqrMagnitude > moveThresholdForChunkUpdateSquared;

        private int chunksVisibleInViewDst;
        public TerrainSettings terrainSettings;
        void Start()
        {
            TerrainChunkProvider = new TerrainChunkProvider(terrainSettings);

            var maxViewDst = terrainSettings.detailLevels[terrainSettings.detailLevels.Length - 1].visibleDstThreshold;
            chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / terrainSettings.meshSettings.meshWorldSize);

            UpdateVisibleChunks();
        }

        void Update()
        {
            currentPosition = new Vector2(transform.position.x, transform.position.z);
            if (IsOnChunkVisibilityEvaluationPosition)
            {
                return;
            }

            foreach (var chunk in visibleTerrainChunks)
            {
                chunk.UpdateCollisionMesh(currentPosition);
            }

            if (ShouldReevaluateChunkVisibility)
            {
                UpdateVisibleChunks();
            }
        }

        void UpdateVisibleChunks()
        {
            chunkVisibilityEvaluationPosition = currentPosition;

            var alreadyUpdatedChunkCoords = new HashSet<Coord>();
            for (var i = visibleTerrainChunks.Count - 1; i >= 0; i--)
            {
                alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
                visibleTerrainChunks[i].UpdateTerrainChunk(currentPosition);
            }

            var currentChunkCoordX = Mathf.RoundToInt(currentPosition.x / terrainSettings.meshSettings.meshWorldSize);
            var currentChunkCoordY = Mathf.RoundToInt(currentPosition.y / terrainSettings.meshSettings.meshWorldSize);

            for (var yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
            {
                for (var xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
                {
                    var viewedChunkCoord = new Coord(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);
                    if (alreadyUpdatedChunkCoords.Contains(viewedChunkCoord))
                    {
                        continue;
                    }

                    // TODO
                    var lodIndex = 0;
                    var chunk = TerrainChunkProvider.GetChunkAsync(null /*parent*/, (int) viewedChunkCoord.x, (int) viewedChunkCoord.y, lodIndex);
                    //chunk.onVisibilityChanged -= OnTerrainChunkVisibilityChanged;
                    //chunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;

                    //chunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
                    //chunk.Load();

                    //if (terrainChunkDictionary.ContainsKey(viewedChunkCoord))
                    //{
                    //    terrainChunkDictionary[viewedChunkCoord].UpdateTerrainChunk();
                    //}
                    //else
                    //{
                    //    var newChunk = new TerrainChunk(viewedChunkCoord, meshSettings, detailLevels, colliderLODIndex, transform, viewer, mapMaterial);
                    //    terrainChunkDictionary.Add(viewedChunkCoord, newChunk);
                    //    newChunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
                    //    newChunk.Load();
                    //}
                }
            }
        }

        private void OnTerrainChunkVisibilityChanged(TerrainChunk chunk, bool isVisible)
        {
            if (isVisible)
            {
                visibleTerrainChunks.Add(chunk);
            }
            else
            {
                visibleTerrainChunks.Remove(chunk);
            }
        }

    }

}