using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] GameObject spikes;
    [SerializeField] float moveDistance = 2f;
    [SerializeField] float moveSpeed = 0.15f;
    [SerializeField] float waitTime = 0.5f;
    [SerializeField] int damageAmount = 10;

    private Vector3 startPos;
    private Vector3 endPos;

    private bool hasDealtDamage = false;

    private void Start()
    {
        startPos = spikes.transform.localPosition;

        // Moves spikes downward
        endPos = startPos - new Vector3(0, moveDistance, 0);

        // Starts each trap at a random time
        StartCoroutine(StartWithDelay());
    }

    private IEnumerator StartWithDelay()
    {
        float randomDelay = Random.Range(0f, 2f);

        yield return new WaitForSeconds(randomDelay);

        StartCoroutine(MoveSpikesLoop());
    }

    private IEnumerator MoveSpikesLoop()
    {
        while (true)
        {
            hasDealtDamage = false;

            // Move down
            yield return MoveSpikes(startPos, endPos);

            yield return new WaitForSeconds(waitTime);

            // Move back up
            yield return MoveSpikes(endPos, startPos);

            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator MoveSpikes(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;

        while (elapsed < moveSpeed)
        {
            spikes.transform.localPosition =
                Vector3.Lerp(from, to, elapsed / moveSpeed);

            elapsed += Time.deltaTime;

            yield return null;
        }

        spikes.transform.localPosition = to;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !hasDealtDamage)
        {
            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damageAmount);

                hasDealtDamage = true;
            }
        }
    }
}