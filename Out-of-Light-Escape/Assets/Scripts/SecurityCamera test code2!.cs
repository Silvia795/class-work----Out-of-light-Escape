using System.Collections;
using UnityEngine;

public class SecurityCamera2 : MonoBehaviour
{
    // This code is a test code for the SecurityCamera model

    [Range(5f, 90f)][SerializeField] float maxAngle = 90f;
    [Range (10f, 30f)][SerializeField] float minTime = 10f;
    [Range(10f, 30f)][SerializeField] float maxTime = 20f;
    [Range(0.25f, 2f)][SerializeField] float detectionTimer;
    [SerializeField] Transform cameraPivot; 

    private bool playerDetected;
    float playerDetectTimer;

    Quaternion startRotation;
    void Start()
    {
        startRotation = cameraPivot.localRotation;
        StartCoroutine(SwivelRoutine());
    }

    void Update()
    {
        if (playerDetected)
        {

        }
    }

    IEnumerator SwivelRoutine()
    {
        while (true)
        {
            yield return RotateTo(-maxAngle);
            yield return new WaitForSeconds(0.5f);
            yield return RotateTo(maxAngle);
            yield return new WaitForSeconds(0.5f);

        }
    }

IEnumerator RotateTo(float angle)
    {
        float duration = Random.Range(minTime, maxTime);
        Quaternion startRot = cameraPivot.localRotation;
        Quaternion targetRot = cameraPivot.localRotation * Quaternion.Euler(0, angle, 0);

        float time = 0f;

        while (time < duration)
        {
            cameraPivot.localRotation = Quaternion.Slerp(startRot, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        cameraPivot.localRotation = targetRot;
    }


    public void playerFound()
    {
        playerDetected = true;
    }


    public void playerlost()
    {
        playerDetected = false;
    }

}
