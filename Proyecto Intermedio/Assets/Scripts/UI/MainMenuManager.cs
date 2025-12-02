using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        GameSceneManager.Instance.LoadGameplay();
    }

    public void QuitGame()
    {
        GameSceneManager.Instance.ExitGame();
    }
}
