using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Collectibles")]
    public int collectibles = 0;
    public int score = 0;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void AddCollectible(int amount)
    {
        collectibles++;
        score += amount;
        Debug.Log($"Collectibles: {collectibles} | Score: {score}");
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("El jugador ha muerto");
    }
}
