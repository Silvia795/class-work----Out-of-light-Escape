using UnityEngine;

public class RotateObjects : MonoBehaviour
{
    public enum RotDirection
    {
        X, Y, Z
    }

    [SerializeField] Transform model;
    [SerializeField] int rotateSpeed;
    [SerializeField] RotDirection rotateDirection;

    // Update is called once per frame
    void Update()
    {
        Vector3 axis = Vector3.up;

        switch (rotateDirection)
        {
            case RotDirection.X:
                axis = Vector3.right;
                break;
            case RotDirection.Y:
                axis = Vector3.up;
                break;
            case RotDirection.Z:
                axis = Vector3.forward;
                break;
        }

        model.transform.RotateAround(model.position, axis, Time.deltaTime * rotateSpeed);
    }
}