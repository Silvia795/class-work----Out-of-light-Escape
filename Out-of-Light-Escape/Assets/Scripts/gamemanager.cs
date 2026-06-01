using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuDeath;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject reticle;
    [SerializeField] TMP_Text gameGoalCountText;
    [SerializeField] Image detectBarLeft;
    [SerializeField] Image detectBarRight;

    [SerializeField] TMP_Text chargeText;

    private bool gameEnded;
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
        if (gameEnded)
            return;

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

        Input.ResetInputAxes();

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
        gameEnded = true;

        if (menuActive != null)
            menuActive.SetActive(false);

        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }
    public void updateGameGoal(int amount)
    {
        gameGoalCount += amount;
        gameGoalCountText.text = gameGoalCount.ToString("F0");
    }

    public void increaseAllDetectionRange()
    {
        enemyAI[] enemies = FindObjectsByType<enemyAI>(FindObjectsSortMode.None);
        int i = 0;
        while (i < enemies.Length)
        {
            enemies[i].increaseDetection();
            i++;
        }
    }

    public void UpdateAmmoUI(int currentAmmo, int maxAmmo, int ammoReserve)
    {
        if (chargeText != null)
            chargeText.text = $"{currentAmmo} / {maxAmmo}   :   {ammoReserve}";
    }



    public void openSettingsMenu()
    {
        if (menuActive != null)
            menuActive.SetActive(false);

        menuActive = menuSettings;
        menuActive.SetActive(true);

        statePause();
    }

    public void backToPauseMenu()
    {
        if (menuActive != null)
            menuActive.SetActive(false);

        menuActive = menuPause;
        menuActive.SetActive(true);

        statePause();
    }
}
