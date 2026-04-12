using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject mainMenuRoot;
    [SerializeField] GameObject gameRoot;

    void Start()
    {
        mainMenuRoot.SetActive(true);
        gameRoot.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        Debug.Log("START CLICKED");
        mainMenuRoot.SetActive(false);
        gameRoot.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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