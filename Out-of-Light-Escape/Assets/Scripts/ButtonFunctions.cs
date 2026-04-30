using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{

    [SerializeField] GameObject gameRoot;
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
}
