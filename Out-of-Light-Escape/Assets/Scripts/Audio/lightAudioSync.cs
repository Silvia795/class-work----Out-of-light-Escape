using UnityEngine;

public class LightAudioSync : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light targetLight;
    [SerializeField] private AudioSource audioSource;

    [Header("Light Settings")]
    [SerializeField] private float minIntensity = 0f;
    [SerializeField] private float maxIntensity = 3f;

    [Header("Audio Reaction")]
    [SerializeField] private float sensitivity = 40f;
    [SerializeField] private float smoothSpeed = 10f;

    private float[] samples = new float[64];

    void Awake()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();
    }

    void Update()
    {
        if (targetLight == null || audioSource == null)
            return;

        audioSource.GetOutputData(samples, 0);

        float volume = 0f;

        for (int i = 0; i < samples.Length; i++)
        {
            volume += Mathf.Abs(samples[i]);
        }

        volume /= samples.Length;

        float targetIntensity = Mathf.Clamp(volume * sensitivity, minIntensity, maxIntensity);

        targetLight.intensity = Mathf.Lerp(
            targetLight.intensity,
            targetIntensity,
            Time.deltaTime * smoothSpeed
        );
    }
}