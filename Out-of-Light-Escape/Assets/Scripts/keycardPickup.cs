using UnityEngine;

public class keycardPickup : MonoBehaviour
{
   [SerializeField] string keycardID = "Red";
    [SerializeField] GameObject promptText;

    bool playerInRange;
    keycardInventory currentInventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            currentInventory = other.GetComponent<keycardInventory>();

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

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (currentInventory != null)
            {
                currentInventory.addKeycard(keycardID);
            }

            if (promptText != null)
            {
                promptText.SetActive(false);
            }

            Destroy(gameObject);
        }
    }
}
