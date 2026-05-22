using System.Runtime.CompilerServices;
using UnityEngine;

public class readNote : MonoBehaviour
{
    [SerializeField] GameObject notePanel;
    [SerializeField] GameObject promptText;

    bool playerInRange;
    bool noteOpen;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            noteOpen = !noteOpen;

            if (notePanel != null)
            {
                notePanel.SetActive(noteOpen);
            }

            if (promptText != null)
            {
                promptText.SetActive(!noteOpen);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (!noteOpen && promptText != null)
            {
                promptText.SetActive(true);
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

            if (notePanel != null)
            {
                notePanel.SetActive(false);
            }

            noteOpen = false;
        }
    }
}
