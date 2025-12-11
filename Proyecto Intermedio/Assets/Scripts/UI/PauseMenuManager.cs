using UnityEngine;
using DG.Tweening;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup pauseGroup;
    [SerializeField] private CanvasGroup gameOverGroup;
    [SerializeField] private float fadeDuration = 0.25f;

    private Tween _fadeTween;
    
    private void OnDisable()
    {
        _fadeTween?.Kill();
    }
    
    public void PauseGame()
    {
        _fadeTween?.Kill();

        pauseGroup.gameObject.SetActive(true);
        pauseGroup.blocksRaycasts = true;
        pauseGroup.interactable = true;

        pauseGroup.alpha = 0f;

        Time.timeScale = 0f;

        _fadeTween = pauseGroup
            .DOFade(1f, fadeDuration)
            .SetUpdate(true); 
    }

    public void ResumeGame()
    {
        _fadeTween?.Kill();

        _fadeTween = pauseGroup
            .DOFade(0f, fadeDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                pauseGroup.blocksRaycasts = false;
                pauseGroup.interactable = false;
                pauseGroup.gameObject.SetActive(false);
                Time.timeScale = 1f;
            });
    }

    public void ShowGameOver()
    {
        _fadeTween?.Kill();

        gameOverGroup.gameObject.SetActive(true);
        gameOverGroup.blocksRaycasts = true;
        gameOverGroup.interactable = true;

        gameOverGroup.alpha = 0f;

        Time.timeScale = 0f;

        _fadeTween = gameOverGroup
            .DOFade(1f, fadeDuration)
            .SetUpdate(true); 
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        GameSceneManager.Instance.LoadMainMenu();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        GameSceneManager.Instance.LoadGameplay();
    }

}