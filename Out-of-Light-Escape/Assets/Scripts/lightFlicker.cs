using System.Collections;
using UnityEngine;

public class lightFlicker : MonoBehaviour, IDamage
{
    [Header("References")]
    [SerializeField] private Light targetLight;

    [Header("Settings")]
    [SerializeField] private float offTime = 3f;
    [SerializeField] private float flickerDuration = 0.5f;
    [SerializeField] private float flickerSpeed = 0.05f;

    private bool isDisabled;
    private float originalIntensity;

    void Awake()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        if (targetLight != null)
            originalIntensity = targetLight.intensity;
    }



    private IEnumerator FlickerAndShutdown()
    {
        isDisabled = true;

        float timer = 0f;

        while (timer < flickerDuration)
        {
            targetLight.enabled = !targetLight.enabled;
            yield return new WaitForSeconds(flickerSpeed);
            timer += flickerSpeed;
        }

        targetLight.enabled = false;

        yield return new WaitForSeconds(offTime);

        targetLight.enabled = true;
        targetLight.intensity = originalIntensity;

        isDisabled = false;
    }

    public void takeDamage(int amount)
    {
        if (targetLight == null)
            return;

        if (!isDisabled)
            StartCoroutine(FlickerAndShutdown());
    }
}