using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage, IPickup
{
    [Header("---- Player Components ----")]
    [SerializeField] GameObject gunModel;
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [Header("---- Player Stats ----")]
    [Range(0, 100)][SerializeField] int HP;
    [Range(1, 10) ][SerializeField] int speed;
    [Range(1, 10)] [SerializeField] int sprintMod;
    [Range(1, 10)] [SerializeField] int jumpSpeed;
    [Range(1, 10)] [SerializeField] int jumpMax;
    [Range(1, 10)] [SerializeField] int gravity;

    [Header("----- Enemy Guns ----")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();

    [Header("---- Stun Gun ----")]
    [SerializeField] stunStats stunWeapon;
    [SerializeField] bool hasStunGun;
    [SerializeField] bool usingStunGun;

    [Header("---- Hit Effects ----")]
    [SerializeField] ParticleSystem defaultHitEffect;
    [SerializeField] ParticleSystem enemyHitEffcet;

    int gunListPos;
    int jumpCount;
    int HPOrig;

    float shootTimer;
    float stunShootTimer;
    float stunChargeTimer;

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();

        if (stunWeapon != null)
        {
            hasStunGun = true;
        }

        if (gunList.Count > 0)
        {
            gunListPos = 0;
            usingStunGun = false;
            changeGun();
        }
        else if (hasStunGun)
        {
            usingStunGun = true;
            changeGun();
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        rechargeStunGun();
    }

    void movement()
    {
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.white);
        shootTimer += Time.deltaTime;
        stunShootTimer += Time.deltaTime;
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

        if (Input.GetButton("Fire1"))
        {
            if (usingStunGun)
            {
                shootStunGun();
            }
            else if (gunList.Count > 0 && gunList[gunListPos].ammoCur > 0 && shootTimer >= gunList[gunListPos].shootRate)
            {
                shoot();
            }
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
        Debug.Log("PLAYER SHOOT CALLED");
        shootTimer = 0;
        gunList[gunListPos].ammoCur--;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, gunList[gunListPos].shootDist, ~ignoreLayer))
        {
                if (gunList[gunListPos] != null)
                {
                    Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);
                }

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if(dmg != null)
            {
                dmg.takeDamage(gunList[gunListPos].shootDamage);
            }
        }
    }

            void shootStunGun()
   {
        if (!hasStunGun || stunWeapon == null)
        {
            return;
        }

        if (stunWeapon.chargeCur < stunWeapon.chargeUse)
        {
            return;
        }

        if (shootTimer < stunWeapon.shootRate)
        {
            return;
        }

        shootTimer = 0;
        stunWeapon.chargeCur -= stunWeapon.chargeUse;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, stunWeapon.shootDist, ~ignoreLayer))
        {
            if (stunWeapon.hitEffect != null)
            {
                Instantiate(stunWeapon.hitEffect, hit.point, Quaternion.identity);
            }

            enemyAI enemy = hit.collider.GetComponentInParent<enemyAI>();
            if (enemy != null)
            {
                enemy.applyStun();
            }
        }
    }

    void rechargeStunGun()
        {
            if (!hasStunGun || stunWeapon == null)
            {
                return;
            }

            stunChargeTimer += Time.deltaTime;

            if (stunChargeTimer < stunWeapon.chargeFillDelay)
            {
                return;
            }

            stunChargeTimer = 0;
            stunWeapon.chargeCur += stunWeapon.chargeFillAmount;

            if (stunWeapon.chargeCur > stunWeapon.chargeMax)
            {
                stunWeapon.chargeCur = stunWeapon.chargeMax;
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

    public void getStunGunStats(stunStats gun)
    {
        Debug.Log("getStunGunStats called");
        stunWeapon = gun;
        hasStunGun = true;
        usingStunGun = true;
        stunWeapon.chargeCur = stunWeapon.chargeMax;
        stunChargeTimer = 0;
        stunShootTimer = 999f;

        Debug.Log("stunShootTimer set to: " + stunShootTimer);
        changeGun();
    }
    void changeGun()
    {
        if (usingStunGun)
        {
            if (stunWeapon != null && stunWeapon.gunModel != null)
            {
                gunModel.GetComponent<MeshFilter>().sharedMesh = stunWeapon.gunModel.GetComponent<MeshFilter>().sharedMesh;
                gunModel.GetComponent<MeshRenderer>().sharedMaterial = stunWeapon.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
            }

            return;
        }

        if (gunList.Count == 0)
        {
            return;
        }

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
