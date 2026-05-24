//using UnityEngine;
//using System.Collections;

//public class DetectionBanner : MonoBehaviour
//{
//    public GameObject suspectedBanner;
//    public GameObject detectedBanner;
//    public GameObject enemySuspectedBanner;
//    public GameObject enemyDetectedBanner;

//    private Coroutine suspectedFlash;
//    private Coroutine detectedFlash;
//    private Coroutine enemySuspectedFlash;
//    private Coroutine enemyDetectedFlash;


//    void Update()
//    {
//        float d = gamemanager.instance.currentDetection;

//        // SUSPECTED STATE
//        if (d >= 0.5f && d < 1f)
//        {
//            detectedBanner.SetActive(false);

//            if (!suspectedBanner.activeSelf)
//            {
//                suspectedBanner.SetActive(true);
//                suspectedFlash = StartCoroutine(FlashBanner(suspectedBanner, 0.8f));
//            }
//        }
//        else
//        {
//            StopFlash(ref suspectedFlash, suspectedBanner);
//        }

//        // DETECTED STATE
//        if (d >= 1f)
//        {
//            suspectedBanner.SetActive(false);

//            if (!detectedBanner.activeSelf)
//            {
//                detectedBanner.SetActive(true);
//                detectedFlash = StartCoroutine(FlashBanner(detectedBanner, 0.4f));
//            }
//        }
//        else
//        {
//            StopFlash(ref detectedFlash, detectedBanner);
//        }

//        // NOTHING
//        if (d < 0.5f)
//        {
//            suspectedBanner.SetActive(false);
//            detectedBanner.SetActive(false);
//        }

//        // ENEMY SUSPECTED STATE
//        bool enemySuspected = false;

//        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

//        foreach (GameObject enemy in enemies)
//        {
//            enemyAI ai = enemy.GetComponent<enemyAI>();

//            if (ai != null && ai.detectionAmount >= 0.5f)
//            {
//                enemySuspected = true;
//                break;
//            }
//        }

//        if (enemySuspected)
//        {
//            if (!enemySuspectedBanner.activeSelf)
//            {
//                enemySuspectedBanner.SetActive(true);
//                enemySuspectedFlash = StartCoroutine(FlashBanner(enemySuspectedBanner, 0.8f));
//            }
//        }
//        else
//        {
//            StopFlash(ref enemySuspectedFlash, enemySuspectedBanner);
//        }

//        // ENEMY DETECTED CHECK
//        bool enemyDetected = false;

//        foreach (GameObject enemy in enemies)
//        {
//            enemyAI ai = enemy.GetComponent<enemyAI>();

//            if (ai != null && ai.detectionAmount >= 1f)
//            {
//                enemyDetected = true;
//                break;
//            }
//        }

//        if (enemyDetected)
//        {
//            if (!enemyDetectedBanner.activeSelf)
//            {
//                enemyDetectedBanner.SetActive(true);
//                enemyDetectedFlash = StartCoroutine(FlashBanner(enemyDetectedBanner, 0.4f));
//            }
//        }
//        else
//        {
//            StopFlash(ref enemyDetectedFlash, enemyDetectedBanner);
//        }
//    }

//    IEnumerator FlashBanner(GameObject banner, float speed)
//    {
//        CanvasGroup cg = banner.GetComponent<CanvasGroup>();

//        if (cg == null)
//        {
//            cg = banner.AddComponent<CanvasGroup>();
//        }

//        while (true)
//        {
//            // fade out
//            yield return Fade(cg, 1f, 0.3f, speed);

//            // fade in
//            yield return Fade(cg, 0.3f, 1f, speed);
//        }
//    }

//    IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
//    {
//        float t = 0f;

//        while (t < duration)
//        {
//            t += Time.deltaTime;
//            cg.alpha = Mathf.Lerp(from, to, t / duration);
//            yield return null;
//        }
//    }

//    void StopFlash(ref Coroutine routine, GameObject banner)
//    {
//        if (routine != null)
//        {
//            StopCoroutine(routine);
//            routine = null;
//        }

//        if (banner != null)
//        {
//            banner.SetActive(false);

//            CanvasGroup cg = banner.GetComponent<CanvasGroup>();
//            if (cg != null)
//                cg.alpha = 1f;
//        }
//    }



//}
