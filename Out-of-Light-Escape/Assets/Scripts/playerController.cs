using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage, IPickup
{
    [SerializeField] CharacterController controller;
    [Range(0, 100)] [SerializeField] int HP;
    [SerializeField] LayerMask ignoreLayer;
    [Range(1, 10) ][SerializeField] int speed;
    [Range(1, 10)] [SerializeField] int sprintMod;
    [Range(1, 10)] [SerializeField] int jumpSpeed;
    [Range(1, 10)] [SerializeField] int jumpMax;
    [Range(1, 10)] [SerializeField] int gravity;
    [SerializeField] GameObject gunModel;
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] ParticleSystem defaultHitEffect;
    [SerializeField] ParticleSystem enemyHitEffcet;
    int gunListPos;
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
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.white);
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
        if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[gunListPos].ammoCur > 0 && shootTimer >= gunList[gunListPos].shootRate)
        {
            shoot();
        }
        selectGun();
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
        gunList[gunListPos].ammoCur--;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, gunList[gunListPos].shootDist, ~ignoreLayer))
        {
            if (gunList[gunListPos] != null)
                Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if(dmg != null)
            {
                dmg.takeDamage(gunList[gunListPos].shootDamage);
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
    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);
        gunListPos = gunList.Count - 1;
        changeGun();






    }
    void changeGun()
    {
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;

    }
    void selectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count - 1)
        {
            gunListPos++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
        {
            gunListPos--;
            changeGun();
        }
    }
}
