using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager instance;

    [Header("UI Sounds")]
    [SerializeField] AudioClip hoverSound;

    [Header("Settings")]
    [Range(0f, 1f)]
    [SerializeField] float volume = 0.6f;

    AudioSource source;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        source = GetComponent<AudioSource>();
    }

    public void PlayHover()
    {
        if (hoverSound != null)
        {
            source.PlayOneShot(hoverSound, volume);
        }
    }
}