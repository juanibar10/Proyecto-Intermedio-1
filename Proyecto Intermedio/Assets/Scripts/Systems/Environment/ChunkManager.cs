using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChunksPool))]
public class ChunkManager : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private ChunksPool chunksPool;
    [SerializeField] private Transform startPoint;
    [SerializeField] private int initialChunks = 3;

    private readonly List<Chunk> _activeChunks = new();
    private int _lastEntryIndex = -1;

    private void OnEnable()
    {
        chunksPool.OnPoolInitialized += GenerateInitialChunks;
        GameEvents.OnChunkReturnToPool += HandleChunkReturned;
    }

    private void OnDisable()
    {
        chunksPool.OnPoolInitialized -= GenerateInitialChunks;
        GameEvents.OnChunkReturnToPool -= HandleChunkReturned;
    }

    private void GenerateInitialChunks()
    {
        for (var i = 0; i < initialChunks; i++)
            SpawnNextChunk(freezeMovement: false);
    }

    private void HandleChunkReturned(Chunk chunk)
    {
        if (_activeChunks.Contains(chunk))
            _activeChunks.Remove(chunk);

        chunksPool.ReturnToPool(chunk);

        // Spawn the next chunk using safe freeze/resume
        StartCoroutine(SpawnSafely());
    }

    private IEnumerator SpawnSafely()
    {
        foreach (var c in _activeChunks)
            c.MovementHandler.active = false;

        yield return null;

        SpawnNextChunk(freezeMovement: true);

        foreach (var c in _activeChunks)
            c.MovementHandler.active = true;
    }

    private void SpawnNextChunk(bool freezeMovement)
    {
        var entries = chunksPool.Entries;
        var count   = entries.Count;

        var index = GetNonRepeatingIndex(count);
        _lastEntryIndex = index;

        var chunk = chunksPool.Get(entries[index].prefab.Data.id);

        if (!chunk)
            return;

        if (freezeMovement)
            chunk.MovementHandler.active = false;

        PositionChunk(chunk);
        _activeChunks.Add(chunk);
    }

    private int GetNonRepeatingIndex(int count)
    {
        if (count <= 1) return 0;

        var index = Random.Range(0, count);
        while (index == _lastEntryIndex)
            index = Random.Range(0, count);

        return index;
    }

    private void PositionChunk(Chunk chunk)
    {
        if (_activeChunks.Count == 0)
        {
            chunk.transform.position = startPoint.position;
            return;
        }

        var last = _activeChunks[^1];
        chunk.transform.position = last.EndPoint.position;
    }
    
    private void OnDrawGizmos()
    {
        var colliders = GetComponentsInChildren<BoxCollider2D>();

        foreach (var col in colliders)
        {
            if (!col.enabled) continue;

            var t = col.transform;
            var scale = t.lossyScale;

            var size = new Vector2(col.size.x * scale.x, col.size.y * scale.y);
            var offset = new Vector2(col.offset.x * scale.x, col.offset.y * scale.y);

            var center = (Vector2)t.position + offset;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
