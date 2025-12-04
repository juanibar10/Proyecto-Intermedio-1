using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public GameObject bulletPrefab;
    public float minShootDelay = 0.2f;   
    public float maxShootDelay = 3.5f;
    
    [Tooltip("Unique identifier for this Enemy type")]
    public int id;
}