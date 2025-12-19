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
        dm.OnDied += HandleDeath;
    }

    private void OnDisable()
    {
        dm.OnDied -= HandleDeath;
    }

    private void HandleDeath()
    {
        // Contar kill SOLO si lo mató el jugador
        if (dm.LastDamageOwner == BulletOwner.Player)
        {
            StatisticsSystem.Instance.AddKill();
        }

        ReturnEnemyToPool();
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