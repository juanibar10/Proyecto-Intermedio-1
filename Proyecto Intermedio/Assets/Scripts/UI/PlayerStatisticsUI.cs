using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerStatisticsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;

    private StatisticsSystem stats;

    void OnEnable()
    {
        StartCoroutine(WaitForStats());
    }

    IEnumerator WaitForStats()
    {
        while (StatisticsSystem.Instance == null)
            yield return null;

        stats = StatisticsSystem.Instance;
        stats.OnStatsChanged += Refresh;
        Refresh();
    }

    void OnDisable()
    {
        if (stats != null)
        {
            stats.OnStatsChanged -= Refresh;
        } 
    }

    private void Refresh()
    {
        var run = stats.CurrentRun;

        if (coinsText != null)
            coinsText.text = run.coinsCollected.ToString();

        if (scoreText != null)
            scoreText.text = run.score.ToString();
    }
}
