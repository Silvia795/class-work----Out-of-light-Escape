using UnityEngine;

[CreateAssetMenu]
public class stunStats : ScriptableObject
{
    public GameObject gunModel;
    [Range(1, 100)] public float shootDist;
    [Range(0.1f, 5f)] public float shootRate;

    [Range(1, 5)]public int startingCharges;
    [Range(1,5)] public int chargesMax;
    [Range(0, 50)] public int chargesReserve;
    [Range(0.1f, 2f)] public float reloadLength;

    public ParticleSystem hitEffect;
}
