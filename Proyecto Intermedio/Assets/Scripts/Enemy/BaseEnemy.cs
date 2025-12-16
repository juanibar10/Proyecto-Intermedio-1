using UnityEngine;

[RequireComponent(typeof(OutOfBoundsNotifier))]
public class BaseEnemy : MonoBehaviour, IOutOfBoundsHandler, IDataProvider<EnemyData>
{
    [SerializeField] private EnemyData data;
    public EnemyData Data => data;
    
    private Damageable dm;

    private void Awake()
    {
        dm = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        dm.OnDied += ReturnEnemyToPool;
    }

    private void OnDisable()
    {
        dm.OnDied -= ReturnEnemyToPool;
    }
    
    public void ReturnEnemyToPool()
    {
        dm.RestoreMaxHP();
        GameEvents.RaiseEnemyReturnToPool(this);
    }

    public void ReturnToPool()
    {
        ReturnEnemyToPool();
    }

}