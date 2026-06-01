using UnityEngine;

public class CellDoor : MonoBehaviour
{
    public GameObject doorObject;

    public bool hasKey;

    public GameObject missingKeyUI;
    public GameObject openDoorUI;

    public float slideDistance = 4f;
    public float slideSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool playerNear;
    private bool doorOpened;

    void Start()
    {
        missingKeyUI.SetActive(false);
        openDoorUI.SetActive(false);

        closedPosition = doorObject.transform.position;

        openPosition = closedPosition + Vector3.up * slideDistance;
    }

    void Update()
    {
        if (playerNear && !hasKey)
        {
            missingKeyUI.SetActive(true);
            openDoorUI.SetActive(false);
        }

        if (playerNear && hasKey && !doorOpened)
        {
            missingKeyUI.SetActive(false);
            openDoorUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                doorOpened = true;
                openDoorUI.SetActive(false);
            }
        }

        if (doorOpened)
        {
            doorObject.transform.position = Vector3.MoveTowards(
                doorObject.transform.position,
                openPosition,
                slideSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            missingKeyUI.SetActive(false);
            openDoorUI.SetActive(false);
        }
    }
}