using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GameOverUI : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Animation")]
    [SerializeField] private float animationDuration = 1.2f;

    [Header("Audio")]
    [SerializeField] private AudioClip tickSound;
    [SerializeField] private float tickInterval = 0.15f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f;
    }

    public void ShowResults(RunStatistics stats)
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateStats(stats));
    }

    private IEnumerator AnimateStats(RunStatistics stats)
    {
        yield return AnimateInt(scoreText, 0, stats.score);
        yield return AnimateInt(coinsText, 0, stats.coinsCollected);
        yield return AnimateInt(killsText, 0, stats.enemiesKilled);
        yield return AnimateFloat(timeText, 0, stats.timeSurvived, " s");
    }

    private IEnumerator AnimateInt(TextMeshProUGUI text, int from, int to)
    {
        float elapsed = 0f;
        float soundTimer = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            soundTimer += Time.unscaledDeltaTime;

            float t = elapsed / animationDuration;
            int value = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
            text.text = value.ToString();

            if (soundTimer >= tickInterval && tickSound != null)
            {
                soundTimer = 0f;
                audioSource.PlayOneShot(tickSound, 0.6f);
            }

            yield return null;
        }

        text.text = to.ToString();
    }

    private IEnumerator AnimateFloat(TextMeshProUGUI text, float from, float to, string suffix)
    {
        float elapsed = 0f;
        float soundTimer = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            soundTimer += Time.unscaledDeltaTime;

            float t = elapsed / animationDuration;
            float value = Mathf.Lerp(from, to, t);
            text.text = $"{value:F1}{suffix}";

            if (soundTimer >= tickInterval && tickSound != null)
            {
                soundTimer = 0f;
                audioSource.PlayOneShot(tickSound, 0.6f);
            }

            yield return null;
        }

        text.text = $"{to:F1}{suffix}";
    }
}
