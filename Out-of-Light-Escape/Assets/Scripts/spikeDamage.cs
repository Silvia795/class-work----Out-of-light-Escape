using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public int damageAmount = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();

            if (player != null)
            {
                player.takeDamage(damageAmount);
            }
        }
    }
}