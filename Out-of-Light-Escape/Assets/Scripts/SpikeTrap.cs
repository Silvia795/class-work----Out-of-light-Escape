using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] GameObject spikes;
    [SerializeField] float resetTimer;
    [SerializeField] int damageAmount;

    bool spikesActive;
    float spikeTimer;
    bool playerNear = false;
    bool hasDealtDamage = false;

    Vector3 startPos = new Vector3(0, 0, 0);
    Vector3 endPos = new Vector3(0, 1, 0);

    void Start()
    {
        startPos = spikes.transform.localPosition;
        endPos = startPos + new Vector3(0, 1, 0);
        spikeTimer += 999f;
    }

    // Update is called once per frame
    void Update()
    {
        spikeTimer += Time.deltaTime;
        if (playerNear && spikeTimer >= resetTimer && !spikesActive)
        {
            StartCoroutine(activateSpikes());
        }
    }

    IEnumerator activateSpikes()
    {
        spikesActive = true;
        spikeTimer = 0;
        hasDealtDamage = false;

        float duration = 0.05f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            spikes.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        spikes.transform.localPosition = endPos;

        yield return new WaitForSeconds(0.5f);

        elapsed = 0f;
        while (elapsed < duration)
        {
            spikes.transform.localPosition = Vector3.Lerp(endPos, startPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        spikes.transform.localPosition = startPos;

        spikesActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            if (spikesActive && !hasDealtDamage)
            {
                IDamage dmg = other.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.takeDamage(damageAmount);
                }
                hasDealtDamage = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (spikesActive && !hasDealtDamage)
            {
                IDamage dmg = other.GetComponent<IDamage>();
                if (dmg != null)
                {
                    dmg.takeDamage(damageAmount);
                }
                hasDealtDamage = true;
            }
        }
    }
}
