using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChunksPool))]
public class ChunkManager : MonoBehaviour
{
    [SerializeField] private ChunksPool chunksPool;
    [SerializeField] private Transform player;
    [SerializeField] private int initialChunks = 3;
    [SerializeField] private float despawnDistance = 30f;

    private readonly List<Chunk> activeChunks = new();
    private bool poolReady;

    private void OnEnable()
    {
        chunksPool.OnPoolInitialized += OnChunksLoaded;
    }

    private void OnDisable()
    {
        chunksPool.OnPoolInitialized -= OnChunksLoaded;
    }

    private void Update()
    {
        if (!poolReady)
            return;

        ManageChunks();
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

    private void ManageChunks()
    {
        if (activeChunks.Count == 0)
            return;

        var first = activeChunks[0];
        var distance = player.position.x - first.transform.position.x;

        if (distance > despawnDistance)
        {
            chunksPool.ReturnToPool(first);
            activeChunks.RemoveAt(0);
        }

        while (activeChunks.Count < initialChunks)
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
