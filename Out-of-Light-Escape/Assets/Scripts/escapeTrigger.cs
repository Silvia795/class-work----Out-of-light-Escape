using UnityEngine;

public class escapeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gamemanager.instance.playerWin();
        }
    }
}
