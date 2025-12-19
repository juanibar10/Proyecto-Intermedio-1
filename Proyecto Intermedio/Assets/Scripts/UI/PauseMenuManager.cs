using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup pauseGroup;
    [SerializeField] private CanvasGroup gameOverGroup;
    [SerializeField] private float fadeDuration = 0.25f;

    [SerializeField] private GameOverUI gameOverUI;

    private Tween _fadeTween;
    private bool _isPaused;

    private InputAction _pauseAction;

    private void Awake()
    {
        _pauseAction = InputSystem.actions.FindAction("Pause");
    }

    private void OnEnable()
    {
        
        _pauseAction?.Enable();
        _pauseAction.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        _fadeTween?.Kill();
        
        if (_pauseAction == null) return;
        
        _pauseAction.performed -= OnPausePerformed;
        _pauseAction?.Disable();
    }

    // ---------------------------------------------------------------------
    // INPUT
    // ---------------------------------------------------------------------
    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        // If game over screen is visible, ignore pause
        if (gameOverGroup != null && gameOverGroup.gameObject.activeSelf)
            return;

        if (_isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    // ---------------------------------------------------------------------
    // PAUSE
    // ---------------------------------------------------------------------
    public void PauseGame()
    {
        if (_isPaused)
            return;

        _isPaused = true;
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
        if (!_isPaused)
            return;

        _isPaused = false;
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

    // ---------------------------------------------------------------------
    // GAME OVER
    // ---------------------------------------------------------------------
    public void ShowGameOver()
    {
        _fadeTween?.Kill();
        _isPaused = true;

        gameOverGroup.gameObject.SetActive(true);
        gameOverGroup.blocksRaycasts = true;
        gameOverGroup.interactable = true;
        gameOverGroup.alpha = 0f;

        Time.timeScale = 0f;

        var stats = StatisticsSystem.Instance.CurrentRun;
        gameOverUI.ShowResults(stats);

        _fadeTween = gameOverGroup
            .DOFade(1f, fadeDuration)
            .SetUpdate(true);
    }

    // ---------------------------------------------------------------------
    // BUTTON CALLBACKS
    // ---------------------------------------------------------------------
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