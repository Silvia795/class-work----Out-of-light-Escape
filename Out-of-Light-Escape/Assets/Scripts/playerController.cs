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

    [Header("---- Crouch ----")]
    [SerializeField] float standingHeight = 2f;
    [SerializeField] float crouchHeight = 1f;
    [SerializeField] int crouchSpeed = 2;

    [Header("----- Enemy Guns ----")]
    [SerializeField] List<gunStats> gunList = new List<gunStats>();

    [Header("---- Stun Gun ----")]
    [SerializeField] stunStats stunWeapon;
    [SerializeField] bool hasStunGun;
    [SerializeField] bool usingStunGun;
    [Range(5, 50)][SerializeField] public int maxReserve;

    [Header("---- Hit Effects ----")]
    [SerializeField] ParticleSystem defaultHitEffect;
    [SerializeField] ParticleSystem enemyHitEffcet;

    int gunListPos;
    int jumpCount;
    int HPOrig;
    int normalSpeed;
    bool isCrouching;
    bool isReloading;

    public float stunShootTimer;
    public int chargesMax;
    public int chargesReserve;
    public float reloadTime;
    public float reloadLength;
    public int chargesCurr;

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        normalSpeed = speed;
        standingHeight = controller.height;
        spawnPlayer();

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
        crouch();

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && hasStunGun && stunWeapon != null
        && chargesCurr < chargesMax && chargesReserve > 0)
        {
            StartCoroutine(ReloadStunGun());
        }
    }

    void crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            controller.height = crouchHeight;
            speed = crouchSpeed;
        }
        else
        {
            isCrouching = false;
            controller.height = standingHeight;
            speed = normalSpeed;
        }
    }

    public void spawnPlayer()
    {
        controller.transform.position = gamemanager.instance.playerSpawnPos.transform.position;
        Physics.SyncTransforms();
        HP = HPOrig;
        updatePlayerUI();
    }

    void movement()
    {
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.white);
        //shootTimer += Time.deltaTime;
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
            else if (gunList.Count > 0 && gunList[gunListPos].ammoCur > 0 )
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
        //shootTimer = 0;
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
        if (!hasStunGun || stunWeapon == null || isReloading)
        {
            return;
        }

        if (chargesCurr <= 0)
        {
            return;
        }

        if (stunShootTimer < stunWeapon.shootRate)
        {
            return;
        }

        stunShootTimer = 0;

        chargesCurr--;

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
        gamemanager.instance.UpdateAmmoUI(chargesCurr, chargesMax, chargesReserve);
    }

    IEnumerator ReloadStunGun()
    {
        isReloading = true;

        // Add animation here

        yield return new WaitForSeconds(reloadLength);

        int needed = chargesMax - chargesCurr;
        int toReload = Mathf.Min(needed, chargesReserve);

        chargesCurr += toReload;
        chargesReserve -= toReload;

        isReloading = false;
        gamemanager.instance.UpdateAmmoUI(chargesCurr, chargesMax, chargesReserve);
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

    public void Heal (int amount)
    {
        HP += amount;

        if(HP > HPOrig)
        {
            HP = HPOrig;
        }

        updatePlayerUI();

        Debug.Log("Player healed. Current HP: " + HP);
    }

    //public getter for health pickups
    public int GetHP() 
    { 
        return HP; 
    }

    public int GetMaxHP()
    {
        return HPOrig;
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
        gamemanager.instance.UpdateAmmoUI(chargesCurr, chargesMax, chargesReserve);
    }

    public void getStunGunStats(stunStats gun)
    {
        Debug.Log("getStunGunStats called");
        stunWeapon = gun;
        hasStunGun = true;
        usingStunGun = true;
        chargesMax = gun.chargesMax;
        reloadLength = gun.reloadLength;
        chargesCurr = gun.startingCharges;
        chargesReserve = gun.chargesReserve;
        stunShootTimer = 999f;

       Debug.Log("stunShootTimer set to: " + stunShootTimer);
        changeGun();
        gamemanager.instance.UpdateAmmoUI(chargesCurr, chargesMax, chargesReserve);
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

    public bool playerHasStunGun()
    {
        if(hasStunGun)
        {
            return true;
        }
        return false;
    }
}
