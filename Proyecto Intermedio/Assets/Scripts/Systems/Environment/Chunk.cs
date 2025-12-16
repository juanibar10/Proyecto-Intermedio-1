using UnityEngine;

public class Chunk : MonoBehaviour, IOutOfBoundsHandler, IDataProvider<ChunkData>
{
    [Header("Chunk Data")]
    [SerializeField] private ChunkData data;
    public ChunkData Data => data;
    
    [Header("Chunk Connection Points")]
    [SerializeField] private Transform endPoint;

    public Transform EndPoint => endPoint;

    [Header("Components")] 
    [SerializeField] private MovementHandler movementHandler;
    public MovementHandler MovementHandler => movementHandler;
    
    public void ReturnToPool()
    {
        GameEvents.RaiseChunkReturnToPool(this);
    }
    
}