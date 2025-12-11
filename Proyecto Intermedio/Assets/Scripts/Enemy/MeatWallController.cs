using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeatWallController : MonoBehaviour
{
    [SerializeField] private bool onlyDamagePlayer = true;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (onlyDamagePlayer && !other.CompareTag(playerTag))
            return;

        Damageable dmg = other.GetComponent<Damageable>();
        if (dmg == null) dmg = other.GetComponentInParent<Damageable>();
        if (dmg == null) dmg = other.GetComponentInChildren<Damageable>();
        if (dmg == null) return;

        dmg.TakeDamage(dmg.MaxHealth, BulletOwner.Enemy);
    }
}
