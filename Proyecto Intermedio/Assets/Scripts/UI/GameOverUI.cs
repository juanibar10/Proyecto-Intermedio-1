using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Button restartButton;

    private void OnEnable()
    {
        gameObject.SetActive(false);
    }

    public void ShowGameOver(RunStatistics finalStats)
    {
        gameObject.SetActive(true);

        if (scoreText != null)
            scoreText.text = "Puntaje: " + finalStats.score;

        if (coinsText != null)
            coinsText.text = "Monedas: " + finalStats.coinsCollected;

        if (killsText != null)
            killsText.text = "Enemigos eliminados: " + finalStats.enemiesKilled;

        if (timeText != null)
            timeText.text = "Tiempo sobrevivido: " + finalStats.timeSurvived.ToString("0.0") + "s";

        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
                );
            });
        }
    }
}
