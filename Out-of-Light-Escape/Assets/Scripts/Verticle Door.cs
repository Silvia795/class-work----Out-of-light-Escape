using UnityEngine;

public class VerticalDoor : MonoBehaviour
{
    [SerializeField] float openHeight = 4f;
    [SerializeField] float speed = 2f;

    Vector3 closedPos;
    Vector3 openPos;

    bool isOpen;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + Vector3.up * openHeight;
    }

    void Update()
    {
        Vector3 target = isOpen ? openPos : closedPos;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}