using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Damageable dmg = other.GetComponent<Damageable>();

        if (dmg != null)
        {
            dmg.TakeDamage(damage, BulletOwner.Enemy);
            Debug.Log("El jugador fue dañado por tocar al enemigo");
        }
    }
}