using UnityEngine;

public class LaserTripwire : MonoBehaviour
{
    [SerializeField] int damageAmount = 10;
    [SerializeField] bool triggerAlarm = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damageAmount);
            }

            if (triggerAlarm)
            {
                //Debug.Log("Alarm triggered!");
            }
        }
    }
}