using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] public int restoreAmount;




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController player = other.GetComponent<playerController>();
            if (player != null && player.playerHasStunGun() && player.chargesReserve != player.chargesMax)
            {
                player.chargesReserve += restoreAmount;

                if (player.chargesReserve >= player.maxReserve)
                {
                    player.chargesReserve = player.maxReserve;
                }
                gamemanager.instance.UpdateAmmoUI(player.chargesCurr, player.chargesMax, player.chargesReserve);
                Destroy(gameObject);
            }
        }
    }
}
