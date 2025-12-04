using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour, ICollector
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Collectibles")]
    public int collectibles = 0;
    public int score = 0;

    [Header("UI")]
    public TextMeshProUGUI collectiblesText;

    void Awake()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void Collect(int value)
    {
        collectibles++;
        score += value;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (collectiblesText != null)
        {
            collectiblesText.text = collectibles.ToString();
        }
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
