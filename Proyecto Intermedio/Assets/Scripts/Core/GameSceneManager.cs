using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using DG.Tweening;
using NaughtyAttributes;

public class GameSceneManager : Singleton<GameSceneManager>
{
    [Header("Scene Names")]
    [SerializeField, Scene] private string mainMenuSceneName = "Main Menu";
    [SerializeField, Scene] private string gameplaySceneName = "Game Scene";

    private string activeScene;
    private FadeSystem fadeSystem;

    private Coroutine loadCoroutine;
    private bool isLoading;

    public bool IsLoading => isLoading;

    protected override void Awake()
    {
        base.Awake();
        DOTween.SetTweensCapacity(500, 50);
    }

    private void Start()
    {
        fadeSystem = FadeSystem.Instance;
        LoadMainMenu();
    }

    // ---------------------------------------------------------------------
    public void LoadMainMenu() => RequestLoad(mainMenuSceneName);
    public void LoadGameplay() => RequestLoad(gameplaySceneName);

    private void RequestLoad(string sceneName)
    {
        if (isLoading)
            return;

        if (loadCoroutine != null)
            StopCoroutine(loadCoroutine);

        loadCoroutine = StartCoroutine(LoadRoutine(sceneName));
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        isLoading = true;
        Time.timeScale = 1;

        yield return fadeSystem.FadeIn().WaitForCompletion();

        // Unload
        if (!string.IsNullOrEmpty(activeScene))
        {
            if (SceneManager.GetSceneByName(activeScene).isLoaded)
            {
                var unload = SceneManager.UnloadSceneAsync(activeScene);
                while (!unload.isDone)
                    yield return null;
            }
        }

        // Load
        var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!load.isDone)
            yield return null;

        activeScene = sceneName;

        var scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);

        yield return fadeSystem.FadeOut().WaitForCompletion();

        isLoading = false;
        loadCoroutine = null;
    }

    public void ExitGame()
    {
        if (isLoading)
            return;

        StartCoroutine(ExitRoutine());
    }

    private IEnumerator ExitRoutine()
    {
        isLoading = true;

        yield return fadeSystem.FadeIn().WaitForCompletion();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
