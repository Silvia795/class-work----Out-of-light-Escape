using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitTrigger : MonoBehaviour
{ 
    [SerializeField] GameObject winScreen;
    [SerializeField] VerticalDoor exitDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(exitSequence());
        }
    }

    IEnumerator exitSequence()
    {
        exitDoor.ToggleDoor();

        yield return new WaitForSeconds(2f);

        winScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
}
