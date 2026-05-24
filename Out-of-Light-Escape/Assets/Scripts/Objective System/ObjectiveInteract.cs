using UnityEngine;

public class ObjectiveInteract : MonoBehaviour
{
    [Header("Objective System")]
    [SerializeField] private ObjectiveText objectiveText;

    [Header("Text Change")]
    [SerializeField] private string updatedObjective = "Objective updated.";

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInRange;
    private bool hasTriggered;

    private void Update()
    {
        if (playerInRange && !hasTriggered && Input.GetKeyDown(interactKey))
        {
            TriggerObjective();
        }
    }

    private void TriggerObjective()
    {
        hasTriggered = true;

        if (objectiveText != null)
            objectiveText.SetObjective(updatedObjective);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}