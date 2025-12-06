using UnityEngine;

public abstract class PowerUpBase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerCollector player = other.GetComponent<PlayerCollector>();

        if (player != null)
        {
            ActivatePowerUp(player);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (transform.position.x < -15f)
            Destroy(gameObject);
    }

    protected abstract void ActivatePowerUp(PlayerCollector player);
}
