using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour, ICollector
{
    [Header("UI")]
    public TextMeshProUGUI collectiblesText;

    private int collectibles = 0;
    private int score = 0;

    void Awake()
    {
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
}
