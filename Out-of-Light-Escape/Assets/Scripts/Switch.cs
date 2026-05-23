using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour
{
    [SerializeField] VerticalDoor door;
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] float openDuration = 3f;
    [SerializeField] GameObject buttonPrompt;
    [SerializeField] bool requiresStunGun = true;

    bool playerInRange;
    bool isRunning;
    playerController player;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey) && !isRunning)
        {
            if (requiresStunGun && player != null && !player.playerHasStunGun())
            {
                if (buttonPrompt != null)
                {
                    buttonPrompt.SetActive(true);
                    buttonPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Stun gun required";
                }

                return;
            }

            StartCoroutine(AutoClose());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.GetComponent<playerController>();

            if (buttonPrompt != null)
            {
                buttonPrompt.SetActive(true);

                if (requiresStunGun && player != null && !player.playerHasStunGun())
                    buttonPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Stun gun required";
                else
                    buttonPrompt.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Press E to interact";
            }
        }
    }

    IEnumerator AutoClose()
    {
        isRunning = true;

        if (buttonPrompt != null)
            buttonPrompt.SetActive(false);

        door.ToggleDoor();

        yield return new WaitForSeconds(openDuration);

        door.ToggleDoor();

        isRunning = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;

            if (buttonPrompt != null)
                buttonPrompt.SetActive(false);
        }
    }
}