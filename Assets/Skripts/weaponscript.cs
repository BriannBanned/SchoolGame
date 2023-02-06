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

    private Animation anim;
    private void Awake()
    {
        anim = transform.parent.GetComponent<Animation>(); //be careful this is sketchy so is everything below
        GameObject _UI = GameObject.FindGameObjectWithTag("UI");
        ammoCountText = _UI.transform.Find("Ammo").transform.Find("AmmoCount").GetComponent<TextMeshProUGUI>();
        reserveCountText = _UI.transform.Find("Ammo").transform.Find("ReserveCount").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        ammoCountText.text = ammo.ToString();
        reserveCountText.text = reserveAmmo.ToString();
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
            if (canShoot == true && ammo > 0)
            {
                print("bang!");
                shoot();  
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            reload();
        }
    }

    public void shoot()
    {
        anim.Stop();
        if (anim.GetClip("gunShoot") != null)
        {
            anim.RemoveClip("gunShoot");
        }
        anim.clip = gunShoot;
        anim.AddClip(gunShoot, "gunShoot");
        anim.Play();
        weaponTimer = weaponShootCoolDown;
        ammo -= 1;
    }
    public void reload()
    {
        if (maxAmmo == ammo && canOverfill == true && reserveAmmo > 0)
        {
            ammo += 1;
            reserveAmmo -= 1;
        }
        else
        {
            if (reserveAmmo > 0 && ammo != maxAmmo + 1)
            {
                if (reserveAmmo < maxAmmo)
                {
                    ammo = reserveAmmo;
                    reserveAmmo = 0;
                }
                else
                {
                    ammo = maxAmmo;
                    reserveAmmo -= maxAmmo;
                }
            }
        }
    }


}
