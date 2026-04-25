using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour
{
    [SerializeField] VerticalDoor door;
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] float openDuration = 3f;

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
        }
    }
    IEnumerator AutoClose()
    {
        isRunning = true;

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
        }
    }
}