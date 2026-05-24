using UnityEngine;
using TMPro;

public class LaserDisableButton : MonoBehaviour
{
    public GameObject laserGroup;
    public GameObject pressEText;

    private bool playerNear;

    private void Start()
    {
        pressEText.SetActive(false);
    }

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            laserGroup.SetActive(false);
            pressEText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            pressEText.SetActive(false);
        }
    }
}