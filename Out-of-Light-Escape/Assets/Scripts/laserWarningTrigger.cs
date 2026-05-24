using UnityEngine;

public class LaserWarningTrigger : MonoBehaviour
{
    public GameObject warningText;

    public bool lasersDisabled;

    private void Start()
    {
        warningText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !lasersDisabled)
        {
            warningText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            warningText.SetActive(false);
        }
    }

    public void DisableLasers()
    {
        lasersDisabled = true;
        warningText.SetActive(false);
    }
}