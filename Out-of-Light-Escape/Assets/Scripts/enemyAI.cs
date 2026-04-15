using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform modelPivot;
    [SerializeField] float knockedDownY = -0.5f;
    [Header("----- Ability Stats -----")]
    [Range(1, 1000)] [SerializeField] int HP;
    [Range(1, 10)] [SerializeField] int targetFaceSpeed;
    [Range(40, 80)] [SerializeField] int FOV;
    
    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [Range(0.1f, 10)] [SerializeField] float shootRate;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform gunPivot;
    [Range(1, 10)] [SerializeField] int gunRotateSpeed;

    [Header("----- Gun Stats -----")]
    [SerializeField] int stunTimer;
    [SerializeField] ParticleSystem stunEffect;
    [SerializeField] Transform particlePos;

    [Header("----- Roaming Stats -----")]
    [Range(1, 500)] [SerializeField] int roamDist;
    [Range(0, 10)] [SerializeField] int roamPauseTime;

    Color colorOrig;
    float shootTimer;
    float angleToPlayer;
    float stoppingDistOrig;
    float roamTimer;

    bool reportedSeeingPlayer = false;
    bool playerInRange;
    bool isStunned;

    Vector3 playerDir;
    Vector3 startingPos;
    Vector3 modelStartPos;
    Quaternion modelStartRot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        gamemanager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        modelStartPos = modelPivot.localPosition;
        modelStartRot = modelPivot.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerInRange && !canSeePlayer())
        {
            checkRoam();
        }
        else if (!playerInRange)
        {
                checkRoam();
        }

        void checkRoam()
        {
             if(agent.remainingDistance < 0.01f)
             {
                    roamTimer += Time.deltaTime;

                    if (roamTimer >= roamPauseTime)
                        roam();
             }
        }

        void roam()
        {
                roamTimer = 0;
                agent.stoppingDistance = 0;

                Vector3 ranPos = Random.insideUnitSphere * roamDist;
                ranPos += startingPos;

                NavMeshHit hit;
                NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
                agent.SetDestination(hit.position);
        }
    }

    bool canSeePlayer()
    {
        playerDir = gamemanager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(transform.position, playerDir);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, playerDir, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {
                //new line of code here for Enemy Detecting Player
                if(!reportedSeeingPlayer)
                {
                    gamemanager.instance.UpdateEnemySeeingPlayer(1);
                    reportedSeeingPlayer = true;
                }

                if (reportedSeeingPlayer) 
                {
                    gamemanager.instance.UpdateEnemySeeingPlayer(-1);
                    reportedSeeingPlayer = false;
                }
              //new code ends here

                if (!isStunned)
                {
                    if (shootTimer >= shootRate)
                    {
                        rotateToTarget();
                        gunRotate();
                        shoot();
                    }
                }
                agent.SetDestination(gamemanager.instance.player.transform.position);
                shootTimer += Time.deltaTime;

                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }
            
        }
        agent.stoppingDistance = 0;
        return false;
    }
    void shoot()
    {
        shootTimer = 0;
        if (bullet != null)
        Instantiate(bullet, shootPos.position, gunPivot.rotation);
    }
    void rotateToTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x,0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }
    void gunRotate()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, rot, Time.deltaTime * gunRotateSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashRed());
            StartCoroutine(stun());
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    IEnumerator stun()
    {
        if (modelPivot != null)
        {
            modelPivot.localRotation = Quaternion.Euler(0f, 0f, 90f);

            Vector3 knockPos = modelStartPos;
            knockPos.y += knockedDownY;
            modelPivot.localPosition = knockPos;
        }

        isStunned = true;
        agent.isStopped = true;
        Instantiate(stunEffect, particlePos.position, particlePos.rotation);

        yield return new WaitForSeconds(stunTimer);

        if (modelPivot != null)
        {
            modelPivot.localRotation = modelStartRot;
            modelPivot.localPosition = modelStartPos;
        }

        agent.isStopped = false;
            isStunned = false;
      
    }
}
