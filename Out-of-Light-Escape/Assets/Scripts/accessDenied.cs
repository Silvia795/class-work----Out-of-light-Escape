using UnityEngine;

public class accessDenied : MonoBehaviour
{
    [SerializeField] GameObject accessDeniedText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (accessDeniedText != null)
            {
                accessDeniedText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (accessDeniedText != null)
            {
                accessDeniedText.SetActive(false);
            }
        }
    }
}
