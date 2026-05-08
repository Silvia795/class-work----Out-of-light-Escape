using UnityEngine;

[CreateAssetMenu]
public class stunStats : ScriptableObject
{
    public GameObject gunModel;
    [Range(1, 100)] public float shootDist;
    [Range(0.1f, 5f)] public float shootRate;
    public int chargeCur;
    [Range(1, 100)] public int chargeMax;
    [Range(1, 100)] public int chargeUse;
    [Range(1, 50)] public int chargeFillAmount;
    [Range(0.1f, 5f)] public float chargeFillDelay;
    public ParticleSystem hitEffect;
}
