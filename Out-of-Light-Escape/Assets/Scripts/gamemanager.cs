using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;


public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuDeath;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuSettings;
    [SerializeField] GameObject reticle;
    [SerializeField] TMP_Text gameGoalCountText;
    [SerializeField] Image detectBarLeft;
    [SerializeField] Image detectBarRight;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider sensitivitySlider;
    public Image playerHPBar;
    public float currentDetection;
    public GameObject playerDamageFlash;
    public bool isPaused;
    public GameObject player;
    public playerController playerScript;
    public cameraController cameraScript;
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

    public void openSettingsMenu()
    {
        menuPause.SetActive(false);
        menuSettings.SetActive(true);
        menuActive = menuSettings;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void backToPauseMenu()
    {
        menuSettings.SetActive(false);
        menuPause.SetActive(true);
        menuActive = menuPause;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

    public void increaseAllDetectionRange()
    {
        enemyAI[] enemies = FindObjectsOfType<enemyAI>();
        int i = 0;
        while(enemies[i] != null)
        {
            i++;
            enemies[i].increaseDetection();
        }
    }

    public void setGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void setMasterVolum(float volume)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void setMouseSensitivity(float sliderValue)
    {
        if (cameraScript != null && sensitivitySlider != null)
        {
            cameraScript.sensitivity = Mathf.Lerp(50f, 300f, sensitivitySlider.value);
        }
    }
}
