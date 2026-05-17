using UnityEngine;

public class swingPendulum : MonoBehaviour
{

    [SerializeField] float swingAngle = 35f;
    [SerializeField] float swingSpeed = 2f;
    [SerializeField] float swingOffset = 0f;

    Quaternion startRotation; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Sin((Time.time * swingSpeed) + swingOffset) * swingAngle;
        transform.localRotation = startRotation * Quaternion.Euler(0, 0, angle);
    }
}
