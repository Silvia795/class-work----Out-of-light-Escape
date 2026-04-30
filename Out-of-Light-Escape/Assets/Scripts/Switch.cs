using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour
{
    [SerializeField] VerticalDoor door;
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] float openDuration = 3f;
    [SerializeField] GameObject button;

    bool playerInRange;
    bool isRunning;
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey) && !isRunning)
        {
            StartCoroutine(AutoClose());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            button.SetActive(true);
        }
    }
    IEnumerator AutoClose()
    {
        isRunning = true;

        door.ToggleDoor();
        button.SetActive(false);

        yield return new WaitForSeconds(openDuration);

        door.ToggleDoor();

        isRunning = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            button.SetActive(false);
        }
    }
}