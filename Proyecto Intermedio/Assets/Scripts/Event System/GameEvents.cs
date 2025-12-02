using System;

public static class GameEvents
{
    /// <summary>Triggered when a chunk is returned to the object pool.</summary>
    public static event Action<Chunk> OnChunkReturnToPool;
    public static void RaiseChunkReturnToPool(Chunk chunk) =>
        OnChunkReturnToPool?.Invoke(chunk);
}
