using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] footstepClips;
    [Range(0f, 1f)][SerializeField] float volume = 0.6f;
    [SerializeField] float pitchMin = 0.9f;
    [SerializeField] float pitchMax = 1.1f;

    [Header("Timing")]
    [SerializeField] float walkStepRate = 0.45f;
    [SerializeField] float runStepRate = 0.28f;
    [SerializeField] float minMoveSpeed = 0.1f;

    float stepTimer;

    public void HandleFootsteps(float moveAmount, bool isRunning, bool isGrounded)
    {
        

        if (!isGrounded)
        {
            stepTimer = 0f;
            return;
        }

        if (moveAmount < minMoveSpeed)
        {
            stepTimer = 0f;
            return;
        }

        float currentRate = isRunning ? runStepRate : walkStepRate;

        stepTimer += Time.deltaTime;

        

        if (stepTimer >= currentRate)
        {
            
            PlayFootstep();
            stepTimer = 0f;
        }
    }

    public void PlayFootstep()
    {
     
        if (source == null || footstepClips.Length == 0)
            return;

        int index = Random.Range(0, footstepClips.Length);

        source.pitch = Random.Range(pitchMin, pitchMax);
        source.PlayOneShot(footstepClips[index], volume);
   
    }
}