using UnityEngine;
using System.Collections;

public class moveObject : MonoBehaviour
{
    [SerializeField] Vector3 moveAmount;
    [SerializeField] float moveSpeed = 2f;

    bool moved;

    public void MoveObject()
    {
        if (!moved)
        {
            moved = true;
            StartCoroutine(moveRoutine());
        }
    }

    IEnumerator moveRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + moveAmount;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
    }
}
