using System.Collections;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    // This code is a test code for the SecurityCamera model

    [Range(5f, 90f)][SerializeField] float maxAngle = 90f;
    [Range (10f, 30f)][SerializeField] float minTime = 10f;
    [Range(10f, 30f)][SerializeField] float maxTime = 20f;

    Quaternion startRotation;

    void Start()
    {
        startRotation = transform.rotation;
        StartCoroutine(SwivelRoutine());
    }

    //void Update()
    //{
    //    DetectPlayer();
   // }

    //void DetectPlayer()
    //{
       // RaycastHit hit;

       // if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, playerLayer))
       // {
          //  Debug.Log("Player Caught!");
           // gamemanager.instance.PlayerCaught();
        //}
    //}
//

    IEnumerator SwivelRoutine()
    {
        while (true)
        {
            yield return RotateTo(-maxAngle);
            yield return RotateTo(0);
            yield return RotateTo(maxAngle);
            yield return RotateTo(0);
        }
    }

IEnumerator RotateTo(float angle)
    {
        float duration = Random.Range(minTime, maxTime);
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = startRotation * Quaternion.Euler(0, angle, 0);

        float time = 0f;

        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
    }
}
