using UnityEngine;
using UnityEngine.AI;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject objToSpawn;
    [SerializeField] int amountToSpawn;
    [SerializeField] int spawnDist;
    [SerializeField] float spawnRate;

    int spawnCount;
    float spawnTimer;

    bool startSpawning;

    void Update()
    {
        if (startSpawning)
        {
            spawnTimer += Time.deltaTime;

            if (spawnCount < amountToSpawn && spawnTimer >= spawnRate)
            {
                spawn();
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
            gamemanager.instance.updateGameGoal(amountToSpawn);
        }
    }

    void spawn()
    {
        spawnTimer = 0;
        spawnCount++;

        Vector3 ranPos = Random.insideUnitSphere * spawnDist;
        ranPos += transform.position;


        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, spawnDist, 1);

        Instantiate(objToSpawn, hit.position, Quaternion.Euler(0f, Random.Range(0, 360), 0));
    }
}
