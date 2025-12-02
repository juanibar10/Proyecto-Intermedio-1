using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Chunk Data")]
    [SerializeField] private ChunkData data;
    public ChunkData Data => data;

    [Header("Chunk Connection Points")]
    [SerializeField] private Transform endPoint;

    public Transform EndPoint => endPoint;

    private void OnValidate()
    {
        if (!data) Debug.LogWarning($"{name}: Missing ChunkData reference.");
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!endPoint) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(endPoint.position, 0.15f);
    }
#endif
}