using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{

    [SerializeField] GameObject gameRoot;

    [Header("Scene Names")]
    [SerializeField] string mainMenuScene = "MainScene";

    public void resume()
    {
        gamemanager.instance.stateResume();
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        gamemanager.instance.stateResume();

    }
    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }

    public void playerRespawn()
    {
        gamemanager.instance.playerScript.spawnPlayer();
        gamemanager.instance.stateResume();
    }

    public void loadLevel(int level)
    {

        if (gamemanager.instance != null)
        {
            gamemanager.instance.stateResume();
        }

        SceneManager.LoadScene(level);

    }
    public void loadMainMenu()
    {
        if (gamemanager.instance != null)
        {
            gamemanager.instance.stateResume();
        }

        SceneManager.LoadScene(mainMenuScene);
    }

    public void openSettingsMenu()
    {
        gamemanager.instance.openSettingsMenu();
    }

    public void backToPauseMenu()
    {
        gamemanager.instance.backToPauseMenu();
    }

}
