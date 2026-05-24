using UnityEngine;

public class LaserBetweenCylinder : MonoBehaviour
{
    public LineRenderer laserLine;
    public Transform laserStartPos;
    public Transform laserEndPos;

    void Update()
    {
        laserLine.SetPosition(0, laserStartPos.position);
        laserLine.SetPosition(1, laserEndPos.position);
    }
}
