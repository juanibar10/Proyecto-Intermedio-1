using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyPool enemyPool;

    public void SpawnEnemy(Vector3 spawnPoint)
    {
        var enemy = enemyPool.Get(0);
        enemy.transform.position = spawnPoint;
    }

    private void DespawnEnemy(BaseEnemy enemy)
    {
        enemyPool.ReturnToPool(enemy);
    }
    
    private void OnEnable()
    {
        GameEvents.OnEnemyReturnToPool += DespawnEnemy;
    }

    private void OnDisable()
    {
        GameEvents.OnEnemyReturnToPool -= DespawnEnemy;
    }

    private void Start()
    {
        SpawnEnemy(transform.position);
    }
}
