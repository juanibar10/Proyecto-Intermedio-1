using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChunksPool))]
public class ChunkManager : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private ChunksPool chunksPool;
    [SerializeField] private Transform startPoint;

    [Space]
    [SerializeField] private int initialChunks = 3;
    [SerializeField] private float worldSpeed = 5f;

    private readonly List<Chunk> activeChunks = new();
    private bool poolReady;

    private int lastEntryIndex = -1;

    private void OnEnable()
    {
        chunksPool.OnPoolInitialized += OnChunksLoaded;
        GameEvents.OnChunkReturnToPool += HandleChunkReturned;
    }

    private void OnDisable()
    {
        chunksPool.OnPoolInitialized -= OnChunksLoaded;
        GameEvents.OnChunkReturnToPool -= HandleChunkReturned;
    }

    private void Update()
    {
        if (!poolReady) return;
        MoveChunks();
    }

    private void OnChunksLoaded()
    {
        poolReady = true;
        GenerateInitialChunks();
    }

    private void GenerateInitialChunks()
    {
        for (var i = 0; i < initialChunks; i++)
            SpawnNextChunk();
    }

    private void MoveChunks()
    {
        var delta = worldSpeed * Time.deltaTime;

        foreach (var chunk in activeChunks)
        {
            var pos = chunk.transform.position;
            pos.x -= delta;
            chunk.transform.position = pos;
        }
    }

    private void HandleChunkReturned(Chunk chunk)
    {
        if (activeChunks.Contains(chunk))
            activeChunks.Remove(chunk);

        chunksPool.ReturnToPool(chunk);
        SpawnNextChunk();
    }

    private void SpawnNextChunk()
    {
        var entries = chunksPool.Entries;
        var count = entries.Count;

        if (count == 0)
        {
            Debug.LogError("No entries in chunk pool.");
            return;
        }

        var index = GetNonRepeatingIndex(count);

        var entry = entries[index];
        lastEntryIndex = index;

        var chunk = chunksPool.Get(entry.prefab.Data.id);
        if (!chunk)
        {
            Debug.LogError("SpawnNextChunk: null chunk returned from pool.");
            return;
        }

        PositionChunk(chunk);
        activeChunks.Add(chunk);
    }

    private int GetNonRepeatingIndex(int count)
    {
        if (count <= 1) return 0;

        var index = Random.Range(0, count);

        while (index == lastEntryIndex)
        {
            index = Random.Range(0, count);
        }

        return index;
    }

    private void PositionChunk(Chunk chunk)
    {
        if (activeChunks.Count == 0)
        {
            chunk.transform.position = startPoint.position;
            return;
        }

        var last = activeChunks[^1];

        if (!last.EndPoint)
        {
            Debug.LogError($"Chunk '{last.name}' has no EndPoint assigned.");
            chunk.transform.position = last.transform.position;
            return;
        }

        chunk.transform.position = last.EndPoint.position;
    }
}