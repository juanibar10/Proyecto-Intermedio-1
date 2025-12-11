using System;
using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    public float duration = 15f;

    public static event Action<string, float> OnPowerUpActivated;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerCollector player = other.GetComponent<PlayerCollector>();

        if (player != null)
        {
            Sprite icon = GetComponent<SpriteRenderer>()?.sprite;

            OnPowerUpActivated?.Invoke(GetType().Name, duration);
            ActivatePowerUp(player);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        //TODO-Utilizar el destroy global
        if (transform.position.x < -15f)
            Destroy(gameObject);
    }

    protected abstract void ActivatePowerUp(PlayerCollector player);
}
