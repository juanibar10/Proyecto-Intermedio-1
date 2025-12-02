using UnityEngine;

public class Chunk : MonoBehaviour, IOutOfBoundsHandler
{
    [Header("Chunk Data")]
    [SerializeField] private ChunkData data;
    public ChunkData Data => data;

    [Header("Chunk Connection Points")]
    [SerializeField] private Transform endPoint;

    public Transform EndPoint => endPoint;
    
    public void ReturnToPool()
    {
        GameEvents.RaiseChunkReturnToPool(this);
    }
}