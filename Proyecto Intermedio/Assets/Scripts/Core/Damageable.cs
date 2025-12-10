using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [SerializeField] public UnityEvent onDamageEvent;

    [SerializeField] private int maxHealth = 5;
    private int currentHealth;

    public int Health => currentHealth;
    public int MaxHealth => maxHealth;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDied;
    

    private void Awake()
    {        
        RestoreMaxHP();
    }

    public void RestoreMaxHP()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount, BulletOwner owner)
    {
        ReceiveDamage(owner, amount);
    }

    public void ReceiveDamage(BulletOwner owner, int damageAmount = 1)
    {
        //Si el ataque proviene de un enemigo y el player tiene escudo, NO recibe da�o
        if (owner is BulletOwner.Enemy or BulletOwner.Environment)
        {
            PlayerCollector player = GetComponent<PlayerCollector>();
            if (player != null && player.IsShieldActive)
                return;
        }  

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onDamageEvent?.Invoke();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnDied?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var damageable = other.gameObject.GetComponent<Damageable>();
        
        if (damageable == null) return;

        if (!other.gameObject.CompareTag("Player")) return;
        
        TakeDamage(1, BulletOwner.Player);
        damageable.TakeDamage(10, BulletOwner.Environment);
    }
}
