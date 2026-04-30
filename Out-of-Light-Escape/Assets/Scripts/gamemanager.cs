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
    [SerializeField] Image detectBarLeft;
    [SerializeField] Image detectBarRight;
    public Image playerHPBar;
    public float currentDetection;
    public GameObject playerDamageFlash;
    public bool isPaused;
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    float timeScaleOrig;
    float detectionFrameMax;
    int gameGoalCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();

        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
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

    void LateUpdate()
    {
        updateDetectionMeter(detectionFrameMax);
        detectionFrameMax = 0f;
    }

    public void reportDetection(float amount)
    {
        amount = Mathf.Clamp01(amount);

        if (amount > detectionFrameMax)
            detectionFrameMax = amount;
    }

    void updateDetectionMeter(float amount)
    {
        currentDetection = amount;

        if (detectBarLeft != null)
            detectBarLeft.fillAmount = amount;

        if (detectBarRight != null)
            detectBarRight.fillAmount = amount;
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
