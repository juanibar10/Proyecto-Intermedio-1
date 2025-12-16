using UnityEngine;

[RequireComponent(typeof(EnemyPool))]
public class EnemyManager : PoolManager<BaseEnemy>
{
    protected override void OnEnable()
    {
        GameEvents.OnEnemyReturnToPool += Despawn;
    }

    protected override void OnDisable()
    {
        GameEvents.OnEnemyReturnToPool -= Despawn;
    }
}