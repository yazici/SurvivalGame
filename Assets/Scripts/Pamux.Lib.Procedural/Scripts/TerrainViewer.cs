using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pamux.Lib.Procedural.Interfaces;
using Pamux.Lib.Procedural.Abstractions;
using Pamux.Lib.Procedural.Data;
using Pamux.Lib.Utilities;
using Pamux.Lib.Managers;
using System.Threading.Tasks;
using System.Threading;

namespace Pamux.Lib.Procedural
{

    public class TerrainViewer : Singleton<TerrainViewer>
    {
        private const float moveThresholdForChunkUpdate = 25f;
        private const float moveThresholdForChunkUpdateSquared = moveThresholdForChunkUpdate * moveThresholdForChunkUpdate;

        private Vector2 currentPosition;
        private Vector2 chunkVisibilityEvaluationPosition;

        // TODO:???
        private IList<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();
        private IDictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();


        private bool IsOnChunkVisibilityEvaluationPosition => currentPosition == chunkVisibilityEvaluationPosition;
        private bool ShouldReevaluateChunkVisibility => (chunkVisibilityEvaluationPosition - currentPosition).sqrMagnitude > moveThresholdForChunkUpdateSquared;

        private int chunksVisibleInViewDst;
        private TerrainSettings terrainSettings;

        public static bool IsReady = false;

        protected override void Awake()
        {
            base.Awake();

            SafeAwakeAsync();
        }

        private async Task SafeAwakeAsync()
        {
            WorldManager.IsReadyEvent.Wait();

            var maxViewDst = terrainSettings.detailLevels[terrainSettings.detailLevels.Length - 1].visibleDstThreshold;
            chunksVisibleInViewDst = Mathf.RoundToInt(maxViewDst / terrainSettings.meshSettings.meshWorldSize);

            currentPosition = new Vector2(transform.position.x, transform.position.z);
            await UpdateVisibleChunksAsync();
            IsReady = true;
        }

        void Update()
        {
            if (!IsReady)
            {
                return;
            }

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
                UpdateVisibleChunksAsync();
            }
        }

        private Task UpdateVisibleChunksAsync()
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
            var tasks = new List<Task>();
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
                    var task = WorldManager.Instance.GetChunkAsync((int)viewedChunkCoord.x, (int)viewedChunkCoord.y, lodIndex);

                    tasks.Add(task.ContinueWith((chunkTask) => {
                        var chunk = chunkTask.Result;
                        chunk.onVisibilityChanged -= OnTerrainChunkVisibilityChanged;
                        chunk.onVisibilityChanged += OnTerrainChunkVisibilityChanged;
                    }));

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
            return Task.WhenAll(tasks);
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