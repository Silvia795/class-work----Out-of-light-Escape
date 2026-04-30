using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string nextSceneName = "GameScene";

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
    }

    //public void StartGame()
    //{
    //    Debug.Log("START CLICKED");

    //    Time.timeScale = 1f;
    //    SceneManager.LoadScene(nextSceneName);
    //}

    public void loadLevel(int level)
    {
        gamemanager.instance.stateResume();

        SceneManager.LoadScene(level);

    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}