using UnityEngine;

public class ventKeyPickup : MonoBehaviour
{
    [SerializeField] GameObject promptText;

    bool playerInRange;
    ventKeyInventory inventory;

    private void Update()
    {
       if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (inventory != null)
            {
                inventory.AddVentKey();
            }

            if (promptText != null)
            {
                promptText.SetActive(false);
            }

            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            inventory = other.GetComponent<ventKeyInventory>();

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
