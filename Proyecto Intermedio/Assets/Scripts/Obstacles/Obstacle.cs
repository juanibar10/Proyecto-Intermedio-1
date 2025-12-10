using System;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Obstacle : MonoBehaviour
{
    private Damageable _damageable;
    public event Action OnDestroyed;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        _damageable.OnDied +=  OnObstacleDestroyed;
    }

    private void OnDisable()
    {
        _damageable.OnDied -=  OnObstacleDestroyed;
    }

    private void OnObstacleDestroyed()
    {
        gameObject.SetActive(false);
        OnDestroyed?.Invoke();
    }

    public void RestoreObstacle()
    {
        _damageable.RestoreMaxHP();
        gameObject.SetActive(true);
    }
}