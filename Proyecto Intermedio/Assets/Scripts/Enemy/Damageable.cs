using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int maxHP = 5;
    [SerializeField] public UnityEvent onDamageEvent;

    private int health;

    public int Health => health;

    private void Awake()
    {        
        RestoreMaxHP();
    }

    public void RestoreMaxHP()
    {
        health = maxHP;
    }

    public void ReceiveDamage(int damageAmount = 1)
    {
        health -= damageAmount;
        Debug.Log("Herido");
        onDamageEvent.Invoke();
    }
    public void TakeDamage(float amount)
    {
        ReceiveDamage((int)amount);
    }
}
