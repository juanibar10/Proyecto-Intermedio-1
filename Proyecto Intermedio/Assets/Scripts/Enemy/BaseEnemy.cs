using System;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    private Damageable dm;

    private void Awake()
    {
        dm = GetComponent<Damageable>();
    }

    private void Update()
    {
        if (dm.Health <= 0)
        {
            SpawnPool.Instance.Despawn(gameObject);
        }
    }
}