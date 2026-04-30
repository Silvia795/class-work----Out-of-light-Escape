using UnityEditor.Timeline.Actions;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    [SerializeField] private ParticleSystem idleParticles;
    [SerializeField] private ParticleSystem activatePartilces;

    private static checkpoint currentCheckpoint;

    private void Start()
    {
       if (idleParticles != null )
        {
            idleParticles.Play();
        }

       if (activatePartilces != null)
        {
            activatePartilces.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (currentCheckpoint == this)
        {
            return;
        }

        if (currentCheckpoint != null)
        {
            currentCheckpoint.TurnIdleOn();
        }

        currentCheckpoint = this;

        gamemanager.instance.playerSpawnPos.transform.position = transform.position;

        TurnIdleOff();

        if (activatePartilces != null)
        {
            activatePartilces.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            activatePartilces.Play();
        }
    }

    private void TurnIdleOn()
    {
        if (idleParticles != null && !idleParticles.isPlaying)
        {
            idleParticles.Play();
        }
    }

    private void TurnIdleOff()
    {
        if (idleParticles != null && idleParticles.isPlaying)
        {
            idleParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}




