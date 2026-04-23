using UnityEngine;

public class cameraPlayerDetections : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("player"))
        {
            
        }
    }



}
