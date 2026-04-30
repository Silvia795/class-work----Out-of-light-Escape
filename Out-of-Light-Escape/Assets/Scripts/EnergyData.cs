using UnityEngine;

public class EnergyData : MonoBehaviour
{
    [CreateAssetMenu]

    public class EnergyPickupData : ScriptableObject
    {
        public int energyAmount = 10;
    }

    
}
