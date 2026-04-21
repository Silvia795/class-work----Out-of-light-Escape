using System;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [Range(0, 100)] [SerializeField] int HP;
    [SerializeField] LayerMask ignoreLayer;
    [Range(1, 10) ][SerializeField] int speed;
    [Range(1, 10)] [SerializeField] int sprintMod;
    [Range(1, 10)] [SerializeField] int jumpSpeed;
    [Range(1, 10)] [SerializeField] int jumpMax;
    [Range(1, 10)] [SerializeField] int gravity;
    [Range(1, 10)] [SerializeField] int shootDamage;
    [Range(1, 10)] [SerializeField] int shootDistance;
    [Range(1, 10)] [SerializeField] float fireRate;
    [SerializeField] ParticleSystem defaultHitEffect;
    [SerializeField] ParticleSystem enemyHitEffcet;

    int jumpCount;
    int HPOrig;

    float shootTimer;

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();


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

        if(Input.GetButton("Fire1") && shootTimer >= fireRate && Time.timeScale >= 0)
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
        shootTimer = 0;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
            if (!hit.collider.CompareTag("Enemy"))
            {
                Instantiate(defaultHitEffect, hit.point, Quaternion.identity);
            }

                IDamage dmg = hit.collider.GetComponent<IDamage>();
            if(dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            gamemanager.instance.playerDeath();
        }
    }
    public void updatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }
    IEnumerator flashDamage()
    {
        gamemanager.instance.playerDamageFlash.SetActive(true);
        yield return new WaitForSecondsRealtime(0.1f);
        gamemanager.instance.playerDamageFlash.SetActive(false);
    }
}
