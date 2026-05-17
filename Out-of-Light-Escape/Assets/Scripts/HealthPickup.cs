using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] int healAmount = 25;

    [Header("Audio")]
    [SerializeField] AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        playerController player = other.GetComponent<playerController>();

        if (player != null )
        {
            player.Heal(healAmount);

            Destroy(gameObject);
        }


    }

}
