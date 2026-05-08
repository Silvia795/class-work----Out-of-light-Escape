using UnityEngine;

public class gunPickupRotate: MonoBehaviour
{
    [SerializeField] Transform model;
    [SerializeField] int rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        model.transform.RotateAround(model.position, Vector3.up, Time.deltaTime * rotateSpeed);
    }
}
