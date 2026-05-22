using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ventUnlock : MonoBehaviour
{
    [SerializeField] GameObject ventGrate;
    [SerializeField] GameObject promptText;
    [SerializeField] GameObject missingKeyText;

    [SerializeField] float openHeight = 2f;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float stayOpenTime = 5f;

    bool playerInRange;
    bool isMoving;
    ventKeyInventory inventory;

    Vector3 closedPos;
    Vector3 openPosl;

    void Start()
    {
        closedPos = ventGrate.transform.position;
        openPosl = closedPos + new Vector3(0, openHeight, 0);
    }

    void Update()
    {
        if (playerInRange && !isMoving && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory != null && inventory.hasVentKey)
            {
                if (promptText != null)
                {
                    promptText.SetActive(false);
                }

                if (missingKeyText != null)
                {
                    missingKeyText.SetActive(false);
                }

                StartCoroutine(OpenAndCloseVent());
            }
            else
            {
                if (missingKeyText != null)
                {
                    missingKeyText.SetActive(true);
                }
            }
        }
    }

    IEnumerator OpenAndCloseVent()
    {
        isMoving = true;
        yield return MoveVent(closedPos, openPosl);

        yield return new WaitForSeconds(stayOpenTime);

        yield return MoveVent(openPosl, closedPos);

        isMoving = false;

        if (playerInRange && promptText != null)
        {
            promptText.SetActive(true);
        }
    }

    IEnumerator MoveVent(Vector3 start, Vector3 end)
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            ventGrate.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        ventGrate.transform.position = end;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            inventory = other.GetComponent<ventKeyInventory>();

            if (!isMoving && promptText != null)
            {
                promptText.SetActive(true);
            }

            if (missingKeyText != null)
            {
                missingKeyText.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (promptText != null)
            {
                promptText.SetActive(false);
            }

            if (missingKeyText != null)
            {
                missingKeyText.SetActive(false);
            }
        }
    }
}
