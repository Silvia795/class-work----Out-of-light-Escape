using UnityEngine;

public class LaserLine : MonoBehaviour
{
    [SerializeField] LineRenderer laserLine;
    [SerializeField] BoxCollider boxCol;
    [SerializeField] float length = 10f;
    [SerializeField] float colliderThickness = 0.2f;
    [SerializeField] LayerMask laserHitMask;

    void Start()
    {
        laserLine.positionCount = 2;
    }

    void Update()
    {
        UpdateLaser();
    }

    void UpdateLaser()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos;

        RaycastHit hit;

        if (Physics.Raycast(startPos, transform.up, out hit, length, laserHitMask))
        {
            endPos = hit.point;
        }
        else
        {
            endPos = startPos + transform.up * length;
        }

        laserLine.SetPosition(0, startPos);
        laserLine.SetPosition(1, endPos);

        float beamLength = Vector3.Distance(startPos, endPos);

        boxCol.center = new Vector3(0, beamLength * 11 + 5, 0);
        boxCol.size = new Vector3(colliderThickness, beamLength * 10, colliderThickness);
    }

}


