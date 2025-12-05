using UnityEngine;

/// <summary>
/// Interface for chunk-specific pool behavior
/// </summary>
public interface IChunkPool : IPool<Chunk> { }
public interface IEnemyPool : IPool<BaseEnemy> { }

[DisallowMultipleComponent]
public class ChunksPool : BasePoolBehaviour<Chunk>, IChunkPool
{
    /// <summary>
    /// Retrieves the unique ID from the prefab to identify pool entries
    /// </summary>
    protected override int GetIdFromPrefab(Chunk prefab) => prefab.Data.id;
    
}