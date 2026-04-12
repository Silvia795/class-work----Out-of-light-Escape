using System;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [Range(0, 100)] [SerializeField] int HP;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] float fireRate;
    [SerializeField] GameObject stunBulletPrefab;
    [SerializeField] Transform shootPos;
    int jumpCount;
    int HPOrig;
    float shootTimer;
    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;

        
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
    }

    void movement()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.white);
        shootTimer += Time.deltaTime;
        if(controller.isGrounded)
        {
            jumpCount = 0;
            playerVel.y = 0;
        }
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir.normalized * speed * Time.deltaTime);
        jump();
        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if(Input.GetButtonDown("Fire1") && shootTimer >= fireRate && Time.timeScale >= 0)
        {
            shoot();
        }
    }

    void sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void jump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpSpeed;
            jumpCount++;
        }
    }

    void shoot()
    {
        Debug.Log("PLAYER SHOOT CALLED");
        shootTimer = 0;

        if (stunBulletPrefab == null || shootPos == null)
        {
            return;
        }

        GameObject spawnedBullet = Instantiate(
            stunBulletPrefab,
            shootPos.position + Camera.main.transform.forward * 1f,
            Camera.main.transform.rotation
        );

        Rigidbody bulletRb = spawnedBullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = Camera.main.transform.forward * 25f;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;

        if(HP <= 0)
        {
            gamemanager.instance.playerDeath();
        }
    }
}
