using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Referencias")]
    public Damageable playerDamageable;
    public Slider healthSlider;
    
    void OnEnable()
    {
        playerDamageable.OnHealthChanged += UpdateUI;
    }

    void OnDisable()
    {
        playerDamageable.OnHealthChanged -= UpdateUI;

    }

    private void Start()
    {
        UpdateUI(playerDamageable.Health, playerDamageable.MaxHealth);
    }

    private void UpdateUI(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
    }
}
