using UnityEngine;

[CreateAssetMenu(fileName = "ChunkData", menuName = "Scriptable Objects/Chunk Data")]
public class ChunkData : ScriptableObject, IPoolData
{
    [SerializeField] private int id;
    public int Id => id;
}