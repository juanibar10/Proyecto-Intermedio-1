using UnityEngine;
using UnityEngine.Events;

public class DamageEffectorMelee : MonoBehaviour
{
    [SerializeField] private UnityEvent onCollision;

    private void OnTriggerEnter(Collider collision)
    {
        onCollision.Invoke();

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            DoDamage(damageable);
        }
    }

    private void DoDamage(IDamageable damageable)
    {
        damageable.TakeDamage(10);
    }
}
