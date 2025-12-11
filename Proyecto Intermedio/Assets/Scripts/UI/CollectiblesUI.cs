using UnityEngine;
using TMPro;

public class CollectiblesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI distanceText;

    private StatisticsSystem stats;

    void OnEnable()
    {
        stats = StatisticsSystem.Instance;

        if (stats != null)
        {
            stats.OnStatsChanged += Refresh;
            Refresh();
        }
    }

    void OnDisable()
    {
        if (stats != null)
            stats.OnStatsChanged -= Refresh;
    }

    private void Refresh()
    {
        var run = stats.CurrentRun;

        if (coinsText != null)
            coinsText.text = run.coinsCollected.ToString();

        if (scoreText != null)
            scoreText.text = run.score.ToString();

        if (killsText != null)
            killsText.text = run.enemiesKilled.ToString();

        if (distanceText != null)
            distanceText.text = run.distanceTravelled.ToString("0.0");
    }
}
