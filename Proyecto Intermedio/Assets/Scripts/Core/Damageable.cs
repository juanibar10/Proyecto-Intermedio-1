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

    public void TakeDamage(int amount)
    {
        ReceiveDamage(amount);
    }

    public void ReceiveDamage(int damageAmount = 1)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onDamageEvent?.Invoke();
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            OnDied?.Invoke();
        }
    }
}
