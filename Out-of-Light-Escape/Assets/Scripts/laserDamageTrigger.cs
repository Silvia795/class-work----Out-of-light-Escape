using UnityEngine;

public class LaserDamageTrigger : MonoBehaviour
{
    public int damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if (dmg != null)
        {
            dmg.takeDamage(damage);
        }
    }
}