using UnityEngine;

public class stunPickup : MonoBehaviour
{
    [SerializeField] stunStats gun;

    private void OnTriggerEnter(Collider other)
    {
        IPickup pick = other.GetComponent<IPickup>();

        if (pick != null)
        {
            pick.getStunGunStats(gun);
            Destroy(gameObject);
        }
    }
}
