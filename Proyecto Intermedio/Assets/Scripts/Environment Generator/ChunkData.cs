using UnityEngine;

[CreateAssetMenu(fileName = "ChunkData", menuName = "Scriptable Objects/Chunk Data")]
public class ChunkData : ScriptableObject
{
    [Tooltip("Unique identifier for this chunk type")]
    public int id;
}