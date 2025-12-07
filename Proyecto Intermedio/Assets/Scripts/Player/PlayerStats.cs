using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerStats : MonoBehaviour, ICollector
{
    [Header("UI")]
    public TextMeshProUGUI collectiblesText;

    private int collectibles = 0;
    private int multiplier = 1;

    void Awake()
    {
        UpdateUI();
    }

    public void Collect()
    {
        collectibles += multiplier;
        UpdateUI();
    }

    public void SetMultiplier(int newMultiplier, float duration)
    {
        StartCoroutine(MultiplierRoutine(newMultiplier, duration));
    }

    private IEnumerator MultiplierRoutine(int newMultiplier, float duration)
    {
        multiplier = newMultiplier;

        yield return new WaitForSeconds(duration);

        multiplier = 1;
    }

    private void UpdateUI()
    {
        if (collectiblesText != null)
        {
            collectiblesText.text = collectibles.ToString();
        }
    }
}
