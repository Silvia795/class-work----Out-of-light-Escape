using UnityEngine;

public class RotateObjects : MonoBehaviour
{
    [SerializeField] Transform model;
    [SerializeField] int rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        model.transform.RotateAround(model.position, Vector3.forward, Time.deltaTime * rotateSpeed);
    }
}
