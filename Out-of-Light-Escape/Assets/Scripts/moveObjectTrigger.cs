using UnityEngine;

public class moveObjectTrigger : MonoBehaviour
{
    [SerializeField] moveObject objectToMove;
    [SerializeField] GameObject promptText;

    bool playerInRange;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            objectToMove.MoveObject();

            if (promptText != null)
            {
                promptText.SetActive(false);
            }

            playerInRange = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (promptText != null)
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
        }
    }
}
