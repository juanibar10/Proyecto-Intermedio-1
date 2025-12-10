using UnityEngine;
using DG.Tweening;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.25f;

    private Tween _fadeTween;
    
    private void OnDisable()
    {
        _fadeTween?.Kill();
    }
    
    public void PauseGame()
    {
        _fadeTween?.Kill();

        canvasGroup.gameObject.SetActive(true);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        canvasGroup.alpha = 0f;

        Time.timeScale = 0f;

        _fadeTween = canvasGroup
            .DOFade(1f, fadeDuration)
            .SetUpdate(true); 
    }

    public void ResumeGame()
    {
        _fadeTween?.Kill();

        _fadeTween = canvasGroup
            .DOFade(0f, fadeDuration)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                canvasGroup.blocksRaycasts = false;
                canvasGroup.interactable = false;
                canvasGroup.gameObject.SetActive(false);
                Time.timeScale = 1f;
            });
    }

    public void GoMainMenu()
    {
        Time.timeScale = 1f;
        GameSceneManager.Instance.LoadMainMenu();
    }
}