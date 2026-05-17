using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource menuSource;
    [SerializeField] AudioSource levelSource;

    [Header("SFX Sources")]
    [SerializeField] AudioSource[] sfxSources;

    [Header("Menu Music")]
    [SerializeField] AudioClip menuMusic;

    [Header("Level Music Playlist")]
    [SerializeField] AudioClip[] levelMusic;

    [Header("Volumes")]
    [Range(0f, 1f)][SerializeField] float masterVolume = 1f;
    [Range(0f, 1f)][SerializeField] float musicVolume = 0.6f;
    [Range(0f, 1f)][SerializeField] float sfxVolume = 0.6f;

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

        LoadVolumes();
        SetupAudioSources();
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
       

        if (SceneManager.GetActiveScene().name == mainMenuSceneName)
            PlayMenuMusic();
        else
            PlayLevelMusic();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        HandleLevelPlaylist();
    }

    void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.6f);
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    void SetupAudioSources()
    {
        menuSource.clip = menuMusic;
        menuSource.loop = true;

        levelSource.loop = false;

        ApplyVolumes();
    }

    void ApplyVolumes()
    {
        AudioListener.volume = masterVolume;

        menuSource.volume = musicVolume;
        levelSource.volume = musicVolume;

        for (int i = 0; i < sfxSources.Length; i++)
        {
            if (sfxSources[i] != null)
                sfxSources[i].volume = sfxVolume;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        

        if (scene.name == mainMenuSceneName)
            PlayMenuMusic();
        else
            PlayLevelMusic();
    }

    void HandleLevelPlaylist()
    {
        if (SceneManager.GetActiveScene().name == mainMenuSceneName)
            return;

        if (!levelSource.isPlaying && levelMusic.Length > 0)
            PlayNextLevelTrack();
    }

    public void PlayMenuMusic()
    {
        if (levelSource.isPlaying)
        {
            levelTime = levelSource.time;
            levelSource.Pause();
        }

        menuSource.volume = musicVolume;

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

        levelSource.volume = musicVolume;

        if (!levelSource.isPlaying)
        {
            if (levelSource.clip == null && levelMusic.Length > 0)
                levelSource.clip = levelMusic[currentLevelTrack];

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
            currentLevelTrack = 0;

        levelSource.clip = levelMusic[currentLevelTrack];
        levelSource.time = 0f;
        levelSource.Play();
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        AudioListener.volume = masterVolume;

        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        menuSource.volume = musicVolume;
        levelSource.volume = musicVolume;

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;

        for (int i = 0; i < sfxSources.Length; i++)
        {
            if (sfxSources[i] != null)
                sfxSources[i].volume = sfxVolume;
        }

        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }
}