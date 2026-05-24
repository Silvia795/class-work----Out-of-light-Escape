using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class laserAlwaysOn : MonoBehaviour
{
    [SerializeField] LineRenderer laserLine;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Transform laserStartPos;
    [SerializeField] int laserMaxDist;
    [SerializeField] int damage;
    [SerializeField] float damageRate;
    [SerializeField] float minTime = 1f;
    [SerializeField] float maxTime = 3f;

    bool isDamaging;
    //bool laserOn = true;
    float ranTimer;

    void Start()
    {
        ranTimer = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        createLaser();
        
    }
    void createLaser()
    {
        if (!laserLine.enabled)
            laserLine.enabled = true;

        laserLine.positionCount = 2;

        RaycastHit hit;

        if (Physics.Raycast(laserStartPos.position, laserStartPos.forward, out hit, laserMaxDist))
        {
            laserLine.SetPosition(0, laserStartPos.position);
            laserLine.SetPosition(1, hit.point);

            hitEffect.SetActive(true);
            hitEffect.transform.position = hit.point;

            if (hit.collider.GetComponentInParent<enemyAI>() != null)
            {
                return;
            }

            IDamage dmg = hit.collider.GetComponentInParent<IDamage>();

            if (dmg != null && !isDamaging)
            {
                StartCoroutine(damageTime(dmg));
            }
        }

        else
        {
            laserLine.SetPosition(0, laserStartPos.position);
            laserLine.SetPosition(1, laserStartPos.position + laserStartPos.forward * laserMaxDist);

            hitEffect.SetActive(false);
        }
    }

    IEnumerator damageTime(IDamage d)
    {
        isDamaging = true;
        d.takeDamage(damage);
        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }
}
    

