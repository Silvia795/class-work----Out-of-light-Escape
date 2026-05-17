using UnityEngine;

public class IdleFloatHealthPickup : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] float floatAmplitude = 0.25f;
    [SerializeField] float floatFrequency = 2f;

    [Header("Rotation")]
    [SerializeField] float rotateSpeed = 50f;

    private Vector3 startPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Float();
        Rotate();
    }

    void Float()
    {
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = startPos + new Vector3(0, newY, 0);
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up * rotateSpeed *  Time.deltaTime);
    }

}
