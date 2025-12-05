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

    protected override void Awake()
    {
        base.Awake();
        DOTween.SetTweensCapacity(500, 50);
    }

    private void Start()
    {
        fadeSystem = FadeSystem.Instance;

        if (fadeSystem == null)
        {
            Debug.LogError("FadeSystem not found in MainScene. Scene loading flow will fail.");
            return;
        }

        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        LoadAdditive(mainMenuSceneName);
    }

    public void LoadGameplay()
    {
        LoadAdditive(gameplaySceneName);
    }

    private void LoadAdditive(string sceneName)
    {
        StartCoroutine(LoadRoutine(sceneName));
    }

    private IEnumerator LoadRoutine(string sceneName)
    {
        // 1. Fade In
        if (fadeSystem)
            yield return fadeSystem.FadeIn().WaitForCompletion();

        // 2. Unload previous scene if exists
        if (!string.IsNullOrEmpty(activeScene))
        {
            var unloadOp = SceneManager.UnloadSceneAsync(activeScene);
            while (unloadOp is { isDone: false })
                yield return null;
        }

        // 3. Load new scene additively
        var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (loadOp != null)
        {
            loadOp.allowSceneActivation = false;

            while (loadOp.progress < 0.9f)
                yield return null;

            loadOp.allowSceneActivation = true;

            while (!loadOp.isDone)
                yield return null;
        }

        activeScene = sceneName;

        var loadedScene = SceneManager.GetSceneByName(sceneName);
        while (!loadedScene.isLoaded)
            yield return null;

        SceneManager.SetActiveScene(loadedScene);

        // 4. Fade Out
        if (fadeSystem)
            yield return fadeSystem.FadeOut().WaitForCompletion();
    }

    public void ExitGame()
    {
        StartCoroutine(ExitRoutine());
    }

    private IEnumerator ExitRoutine()
    {
        if (fadeSystem)
            yield return fadeSystem.FadeIn().WaitForCompletion();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
