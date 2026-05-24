using System.Collections.Generic;
using UnityEngine;

public class keycardInventory : MonoBehaviour
{
    public List<string> keycards = new List<string>();

    public void addKeycard(string keycardID)
    {
        if (!keycards.Contains(keycardID))
        {
            keycards.Add(keycardID);
            //Debug.Log("Picked up keycard: " + keycardID);
        }
    }

    public bool hasKeycard(string keycardID)
    {
        return keycards.Contains(keycardID);
    }

}
