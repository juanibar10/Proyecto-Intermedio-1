using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyPool))]
public class EnemyManager : PoolManager<BaseEnemy>
{
    public IReadOnlyList<EnemyData> AvailableEnemyData => (pool as EnemyPool)?.EntriesData;

    public void Spawn(Vector3 position, EnemyData data)
    {
        var enemy = pool.Get(data.Id);
        enemy.transform.position = position;
    }

    protected override void OnEnable()
    {
        GameEvents.OnEnemyReturnToPool += Despawn;
    }

    protected override void OnDisable()
    {
        GameEvents.OnEnemyReturnToPool -= Despawn;
    }
}