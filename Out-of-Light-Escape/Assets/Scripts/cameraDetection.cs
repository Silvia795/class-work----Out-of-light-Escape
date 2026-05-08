using UnityEngine;
using UnityEngine.UI;

public class cameraDetection : MonoBehaviour
{
    [SerializeField] Transform cameraHead;
    [SerializeField] Transform player;
    [SerializeField] LayerMask viewMask;
    [SerializeField] float scanSpeed = 30f;
    [SerializeField] float scanAngle = 60f;
    Quaternion startRotation;
    float scanTimer;
    [SerializeField] float rotateSpeed = 3f;
    [SerializeField] float detectionSpeed = 0.5f;
    [SerializeField] float loseSpeed = 0.75f;

    float detectionAmount;
    bool playerInRange;

    void Start()
    {
        startRotation = cameraHead.rotation;

    }
    void Update()
    {
        bool canSeePlayer = playerInRange && CanSeePlayer();

        if (canSeePlayer)
        {
            LockOnPlayer();
            detectionAmount += detectionSpeed * Time.deltaTime;
        }
        else
        {
            ScanArea();
            detectionAmount -= loseSpeed * Time.deltaTime;
        }

        detectionAmount = Mathf.Clamp01(detectionAmount);
        UpdateDetectionUI();
    }

    bool CanSeePlayer()
    {
        Vector3 direction = player.position - cameraHead.position;

        RaycastHit hit;

        if (Physics.Raycast(cameraHead.position, direction.normalized, out hit, direction.magnitude, viewMask))
        {
            if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    void LockOnPlayer()
    {
        Vector3 playerDir = player.position - cameraHead.position;

        Quaternion rot = Quaternion.LookRotation(
            new Vector3(playerDir.x, 0, playerDir.z)
        );

        cameraHead.rotation = Quaternion.Lerp(
            cameraHead.rotation,
            rot,
            Time.deltaTime * rotateSpeed
        );
    }

    void UpdateDetectionUI()
    {
        gamemanager.instance.reportDetection(detectionAmount);
    }
    void ScanArea()
    {
        scanTimer += Time.deltaTime * scanSpeed;

        float yOffset = Mathf.Sin(scanTimer) * scanAngle;

        cameraHead.rotation = startRotation * Quaternion.Euler(0f, yOffset, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}