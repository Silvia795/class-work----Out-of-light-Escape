using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    [SerializeField] Transform player;
    [SerializeField] Transform respawnPoint;

    [Header("UI")]
    [SerializeField] CanvasGroup caughtBanner;
    [SerializeField] float fadeDuration = 1f;

    void Awake()
    {
        instance = this;
    }

    public void PlayerCaught()
    {
        StartCoroutine(CaughtSequence());
    }

    IEnumerator CaughtSequence()
    {
        // Fade IN
        yield return StartCoroutine(FadeBanner(0, 1));

        yield return new WaitForSeconds(1.5f);

        // Reset player position
        player.position = respawnPoint.position;

        // Fade OUT
        yield return StartCoroutine(FadeBanner(1, 0));
    }

    IEnumerator FadeBanner(float start, float end)
    {
        float time = 0;

        while (time < fadeDuration)
        {
            caughtBanner.alpha = Mathf.Lerp(start, end, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        caughtBanner.alpha = end;
    }
}