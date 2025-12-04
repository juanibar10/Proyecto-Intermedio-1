using System;
using UnityEngine;

[RequireComponent(typeof(OutOfBoundsNotifier))]
public class BaseEnemy : MonoBehaviour, IOutOfBoundsHandler
{
    public EnemyData data;
    private Damageable dm;

    private void Awake()
    {
        dm = GetComponent<Damageable>();
    }

    private void Update()
    {
        if (dm.Health <= 0)
        {
            ReturnEnemyToPool();
        }
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