using UnityEngine;
using TMPro;

public class LaserWarningTrigger : MonoBehaviour
{
    public GameObject warningText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
}