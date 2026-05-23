using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public static MainMenuController instance;

    [Header("Menus")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject menuCredits;

    [Header("Scene")]
    [SerializeField] string nextSceneName = "GameScene";

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;

        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    public void loadLevel(int level)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(level);
    }

    public void openSettingsMainMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void closeSettingsMainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void openCreditsMainMenu()
    {
        mainMenu.SetActive(false);
        menuCredits.SetActive(true);
    }

    public void closeCreditsMainMenu()
    {
        menuCredits.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}