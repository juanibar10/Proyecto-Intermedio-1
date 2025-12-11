using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("References")]
    public Damageable playerDamageable;
    public Slider healthSlider;

    [Header("Colors of Life")]
    private Color highHealthColor = Color.green;
    private Color midHealthColor = Color.yellow;
    private Color lowHealthColor = Color.red;

    private Image fillImage;

    void Awake()
    {
        fillImage = healthSlider.fillRect.GetComponent<Image>();
    }

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

        float healthPercent = (float) current / max;

        if (healthPercent > 0.5f)
            fillImage.color = highHealthColor;
        else if (healthPercent > 0.2f)
            fillImage.color = midHealthColor;
        else
            fillImage.color = lowHealthColor;
    }
}
