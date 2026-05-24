using UnityEngine;

public class keycardDoor : MonoBehaviour
{
    public string requiredKeycard = "Red";
    public GameObject doorObject;
    public GameObject missingMessage;

    bool hasOpened;

    private void OnTriggerEnter(Collider other)
    {
        keycardInventory inventory = other.GetComponent<keycardInventory>();

        if (inventory != null && !hasOpened)
        {
            if (inventory.hasKeycard(requiredKeycard))
            {
                hasOpened = true;
                StartCoroutine(openAndCloseDoor());
                //Debug.Log("Door Unlocked");
            }
            else
            {
                if (missingMessage != null)
                {
                    StopAllCoroutines();
                    StartCoroutine(showMessage());
                }
            }
        }
    }

    System.Collections.IEnumerator showMessage()
    {
        missingMessage.SetActive(true);

        yield return new WaitForSeconds(2f);

        missingMessage.SetActive(false);
    }

    System.Collections.IEnumerator openAndCloseDoor()
    {
        VerticalDoor door = doorObject.GetComponent<VerticalDoor>();

        door.ToggleDoor();

        yield return new WaitForSeconds(3f);

        door.ToggleDoor();

        hasOpened = false;
    }
}
