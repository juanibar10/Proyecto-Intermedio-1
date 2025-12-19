using System;
using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Obstacle : MonoBehaviour
{
    private Damageable _damageable;
    public event Action OnDestroyed;

    [Header("Audio")]
    [SerializeField] private AudioClip breakSound;

    private void Awake()
    {
        _damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        _damageable.OnDied += OnObstacleDestroyed;
    }

    private void OnDisable()
    {
        _damageable.OnDied -= OnObstacleDestroyed;
    }

    private void OnObstacleDestroyed()
    {
        if (breakSound != null)
            AudioSource.PlayClipAtPoint(breakSound, transform.position);

        gameObject.SetActive(false);
        OnDestroyed?.Invoke();
    }

    public void RestoreObstacle()
    {
        _damageable.RestoreMaxHP();
        gameObject.SetActive(true);
    }
}