using System;
using UnityEngine;

[RequireComponent(typeof(OutOfBoundsNotifier))]
public abstract class PowerUpBase : MonoBehaviour, IOutOfBoundsHandler, IDataProvider<PowerUpData>
{
    [SerializeField] private PowerUpData data;
    public PowerUpData Data => data;

    public static event Action<string, float> OnPowerUpActivated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerCollector player = other.GetComponent<PlayerCollector>();

        if (player != null)
        {
            Sprite icon = GetComponent<SpriteRenderer>()?.sprite;

            OnPowerUpActivated?.Invoke(GetType().Name, data.duration);
            ActivatePowerUp(player);
            ReturnToPool();
        }
    }
    
    protected abstract void ActivatePowerUp(PlayerCollector player);
    
    public void ReturnToPool()
    {
        GameEvents.RaisePowerUpReturnToPool(this);
    }
}
