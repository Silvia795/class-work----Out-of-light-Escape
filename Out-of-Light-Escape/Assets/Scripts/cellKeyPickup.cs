using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameObject keyPickupUI;
    public CellDoor cellDoor;

    private bool playerNear;

    void Start()
    {
        keyPickupUI.SetActive(false);
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            cellDoor.hasKey = true;
            keyPickupUI.SetActive(false);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            keyPickupUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            keyPickupUI.SetActive(false);
        }
    }
}