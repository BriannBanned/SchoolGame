using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class weaponscript : MonoBehaviour
{
    public string weaponid;
    public int ammo;
    public int maxAmmo;
    public int reserveAmmo;
    public int maxReserveAmmo;
    public bool canOverfill;
    public AnimationClip gunIdle;
    public AnimationClip gunShoot;
    public AnimationClip gunReload;
    public bool isSelected = false;
    public float weaponShootCoolDown;

    private bool canShoot = true;
    private float weaponTimer = 0f;
    private TextMeshProUGUI ammoCountText;
    private TextMeshProUGUI reserveCountText;
    private void Awake()
    {
        
    }

    private void Update()
    {
        if (weaponTimer <= 0.0f)
        {
            //can shoot yessir!
            canShoot = true;
        }
        else
        {
            canShoot = false;
            weaponTimer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (canShoot == true)
            {
                print("bang!");
                shoot();  
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (maxAmmo == ammo && canOverfill == true)
            {
                ammo += 1;
            }
            else
            {
                if (reserveAmmo > 0)
                {
                    ammo = maxAmmo;
                    reserveAmmo -= maxAmmo;
                }
            }
        }
    }

    public void shoot()
    {
        weaponTimer = weaponShootCoolDown;
        ammo -= 1;
    }



}
