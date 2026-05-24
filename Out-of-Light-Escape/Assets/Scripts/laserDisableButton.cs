using UnityEngine;

public class LaserDisableButton : MonoBehaviour
{
    public GameObject laserGroup;
    public GameObject pressEText;
    public LaserWarningTrigger laserWarningTrigger;

    private bool playerNear;
    private bool disabled;

    private void Start()
    {
        pressEText.SetActive(false);
    }

    private void Update()
    {
        if (playerNear && !disabled && Input.GetKeyDown(KeyCode.E))
        {
            disabled = true;

            laserGroup.SetActive(false);
            pressEText.SetActive(false);

            laserWarningTrigger.DisableLasers();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !disabled)
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