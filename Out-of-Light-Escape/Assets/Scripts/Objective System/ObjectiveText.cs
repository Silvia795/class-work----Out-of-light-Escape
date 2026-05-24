using TMPro;
using UnityEngine;

public class ObjectiveText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI objectiveText;

    [Header("Objective Text")]
    [SerializeField] private string startingText = "Find a way out of your cell";

    private void Awake()
    {
        if (objectiveText == null)
            objectiveText = GetComponent<TextMeshProUGUI>();

        SetObjective(startingText);
    }

    public void SetObjective(string newText)
    {
        if (objectiveText != null)
            objectiveText.text = newText;
    }
}