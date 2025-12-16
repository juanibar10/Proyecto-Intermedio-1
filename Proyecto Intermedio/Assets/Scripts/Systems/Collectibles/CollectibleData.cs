using UnityEngine;

[CreateAssetMenu(fileName = "CollectibleData", menuName = "Scriptable Objects/Collectible Data")]
public class CollectibleData : ScriptableObject, IPoolData
{
    [SerializeField] private int id;
    public int Id => id;
    
    public float pullSpeed = 25f;
}