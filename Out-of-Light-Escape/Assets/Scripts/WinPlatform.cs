using UnityEngine;

public class WinPlatform : MonoBehaviour
{
    [SerializeField] GameObject platform;


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            platform.SetActive(false);
        }
    }
}
