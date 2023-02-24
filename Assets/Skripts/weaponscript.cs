using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class weaponscript : NetworkBehaviour
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
    public AudioClip[] gunShotSounds = new AudioClip[] { };
    public AudioClip gunReloadSound;
    public bool isAutomatic = false;
    public bool ignoreAnimations;
    public Vector3 spawnOffset;
    
    private bool canShoot = true;
    private float weaponTimer = 0f;
    private TextMeshProUGUI ammoCountText;
    private TextMeshProUGUI reserveCountText;

    private Animation anim;
    private AudioSource audSauce;
    private void Awake()
    {
        audSauce = GetComponent<AudioSource>();
        anim = transform.parent.GetComponent<Animation>(); //be careful this is sketchy so is everything below
        GameObject _UI = GameObject.FindGameObjectWithTag("UI");
        ammoCountText = _UI.transform.Find("Ammo").transform.Find("AmmoCount").GetComponent<TextMeshProUGUI>();
        reserveCountText = _UI.transform.Find("Ammo").transform.Find("ReserveCount").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!IsOwner) return;
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

        if (!isAutomatic)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (canShoot == true && ammo > 0 && !anim.IsPlaying("gunReload"))
                {
                    print("bang!");
                    shoot();
                }
            }
        }
        else
        {
            if (Input.GetButton("Fire1"))
            {
                if (canShoot == true && ammo > 0 && !anim.IsPlaying("gunReload"))
                {
                    print("bang!");
                    shoot();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!anim.IsPlaying("gunReload"))
            {
                reload();
            }
        }
    }

    public void shoot()
    {
        if (ignoreAnimations == false)
        {
            audSauce.PlayOneShot(gunShotSounds[UnityEngine.Random.Range(0, gunShotSounds.Count())]);
            audSauce.Play();
            anim.Stop();
            if (anim.GetClip("gunShoot") != null)
            {
                anim.RemoveClip("gunShoot");
            }
            anim.clip = gunShoot;
            anim.AddClip(gunShoot, "gunShoot");
            anim.Play();  
        }
        weaponTimer = weaponShootCoolDown;
        ammo -= 1;
    }
    public void reload()
    {
        if (ammo > maxAmmo)
        {
            return;
        }
        else if(canOverfill == false && ammo == maxAmmo)
        {
            return;
        }

        if (ignoreAnimations == false)
        {
            audSauce.PlayOneShot(gunReloadSound);
            print("hi");
            anim.Stop();
            if (anim.GetClip("gunReload") != null)
            {
                anim.RemoveClip("gunReload");
            }

            anim.clip = gunReload;
            anim.AddClip(gunReload, "gunReload");
            anim.Play();
        }

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
                    reserveAmmo -= maxAmmo - ammo;
                    ammo = maxAmmo;
                }
            }
        }
    }
}
