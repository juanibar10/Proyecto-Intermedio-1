using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChunksPool))]
public class ChunkManager : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private ChunksPool chunksPool;
    [SerializeField] private int initialChunks = 3;
    [SerializeField] private float worldSpeed = 5f;

    private readonly List<Chunk> activeChunks = new();
    private bool poolReady;

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
        if (!poolReady)
            return;

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
        var randomEntry = entries[Random.Range(0, entries.Count)];

        var chunk = chunksPool.Get(randomEntry.prefab.Data.id);
        if (!chunk)
        {
            Debug.LogError("SpawnNextChunk: null chunk returned from pool.");
            return;
        }

        if (activeChunks.Count == 0)
        {
            chunk.transform.position = Vector3.zero;
        }
        else
        {
            var last = activeChunks[^1];

            if (!last.EndPoint)
            {
                Debug.LogError($"Chunk '{last.name}' has no EndPoint assigned.");
                chunk.transform.position = last.transform.position;
            }
            else
            {
                var spawnPos = last.EndPoint.position;
                chunk.transform.position = spawnPos;
            }
        }

        activeChunks.Add(chunk);
    }
}
