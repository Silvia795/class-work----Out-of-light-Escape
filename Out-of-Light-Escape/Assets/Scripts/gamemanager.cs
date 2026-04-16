using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuDeath;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject reticle;
    [SerializeField] TMP_Text gameGoalCountText;
    public Image playerHPBar;
    public GameObject playerDamageFlash;
    public bool isPaused;
    public GameObject player;
    public playerController playerScript;
    float timeScaleOrig;
    int gameGoalCount;
    public bool isPlayerSafe = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        isPlayerSafe = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateResume();
            }
        }
    }

    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        reticle.SetActive(false);
    }
    public void stateResume()
    {
        isPaused = false;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
        reticle.SetActive(true);
    }
    public void playerDeath()
    {
        statePause();
        menuActive = menuDeath;
        menuActive.SetActive(true);
    }
    public void playerWin()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }
    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        gameGoalCountText.text = gameGoalCount.ToString("F0");
    }

}
