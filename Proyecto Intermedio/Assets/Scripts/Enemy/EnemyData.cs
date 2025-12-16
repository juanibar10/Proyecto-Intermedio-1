using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/Enemy Data")]
public class EnemyData : ScriptableObject, IPoolData
{
    [SerializeField] private int id;
    public int Id => id;
    
    public GameObject bulletPrefab;
    public float minShootDelay = 0.2f;   
    public float maxShootDelay = 3.5f;
}