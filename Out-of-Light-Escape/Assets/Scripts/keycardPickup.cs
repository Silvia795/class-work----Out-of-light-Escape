using UnityEngine;

public class keycardPickup : MonoBehaviour
{
   [SerializeField] string keycardID = "Red";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            keycardInventory inventory = other.GetComponent<keycardInventory>();

            if (inventory != null)
            {
                inventory.addKeycard(keycardID);
                Destroy(gameObject);
            }
        }
    }
}
