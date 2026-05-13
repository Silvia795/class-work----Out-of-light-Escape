using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource menuSource;
    [SerializeField] AudioSource levelSource;

    [Header("Menu Music")]
    [SerializeField] AudioClip menuMusic;

    [Header("Level Music Playlist")]
    [SerializeField] AudioClip[] levelMusic;

    [Header("Volumes")]
    [Range(0f, 1f)][SerializeField] float menuVolume = 0.6f;
    [Range(0f, 1f)][SerializeField] float levelVolume = 0.6f;

    [Header("Scene Names")]
    [SerializeField] string mainMenuSceneName = "MainMenu";

    int currentLevelTrack;

    float menuTime;
    float levelTime;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        SetupAudioSources();
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().name == mainMenuSceneName)
        {
            PlayMenuMusic();
        }
        else
        {
            PlayLevelMusic();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        HandleLevelPlaylist();
    }

    void SetupAudioSources()
    {
        menuSource.clip = menuMusic;
        menuSource.loop = true;
        menuSource.volume = menuVolume;

        levelSource.loop = false;
        levelSource.volume = levelVolume;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuSceneName)
        {
            PlayMenuMusic();
        }
        else
        {
            PlayLevelMusic();
        }
    }

    void HandleLevelPlaylist()
    {
        if (SceneManager.GetActiveScene().name == mainMenuSceneName)
            return;

        if (!levelSource.isPlaying && levelMusic.Length > 0)
        {
            PlayNextLevelTrack();
        }
    }

    public void PlayMenuMusic()
    {
        if (levelSource.isPlaying)
        {
            levelTime = levelSource.time;
            levelSource.Pause();
        }

        menuSource.volume = menuVolume;

        if (!menuSource.isPlaying)
        {
            menuSource.time = menuTime;
            menuSource.Play();
        }
    }

    public void PlayLevelMusic()
    {
        if (menuSource.isPlaying)
        {
            menuTime = menuSource.time;
            menuSource.Pause();
        }

        levelSource.volume = levelVolume;

        if (!levelSource.isPlaying)
        {
            if (levelSource.clip == null && levelMusic.Length > 0)
            {
                levelSource.clip = levelMusic[currentLevelTrack];
            }

            levelSource.time = levelTime;
            levelSource.Play();
        }
    }

    void PlayNextLevelTrack()
    {
        if (levelMusic.Length == 0)
            return;

        currentLevelTrack++;

        if (currentLevelTrack >= levelMusic.Length)
        {
            currentLevelTrack = 0;
        }

        levelSource.clip = levelMusic[currentLevelTrack];
        levelSource.time = 0f;

        levelSource.Play();
    }

    public void SetMenuVolume(float volume)
    {
        menuVolume = volume;
        menuSource.volume = menuVolume;
    }

    public void SetLevelVolume(float volume)
    {
        levelVolume = volume;
        levelSource.volume = levelVolume;
    }
}