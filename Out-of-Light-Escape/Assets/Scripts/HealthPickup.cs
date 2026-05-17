using System.Collections;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] int healAmount = 25;
    [SerializeField] float respawnTimer = 10f;

    [Header("Audio")]
    [SerializeField] AudioClip pickupSound;

    private Collider pickupCollider;
    private Renderer pickupRenderer;

    private bool isCollected = false;

    void Start()
    {
       pickupCollider = GetComponent<Collider>();
       pickupRenderer = GetComponent<Renderer>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        playerController player = other.GetComponent<playerController>();

        if (player != null)
        {

            if (player.GetHP() > player.GetMaxHP())
            {
                return;
            }

            isCollected = true;

            player.Heal(healAmount);

            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            StartCoroutine(RespawnPickup());
        }


    }

    IEnumerator RespawnPickup()
    {
        pickupCollider.enabled = false;
        pickupRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTimer);

        pickupCollider.enabled = true;
        pickupRenderer.enabled = true;

        isCollected = false;
    }


}
