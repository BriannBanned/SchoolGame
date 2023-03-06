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
    [SerializeField] public string weaponID;
    [SerializeField] private NetworkVariable<int> ammo = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> maxAmmo = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> reserveAmmo = new NetworkVariable<int>(0);
    [SerializeField] private bool canOverfill;
    [SerializeField] private bool isAutomatic;
    [SerializeField] private float weaponCoolDown;
    //internal junk
    private bool canShoot = true;
    private float timerGun;
    private TextMeshProUGUI textReserve;
    private TextMeshProUGUI textAmmo;

    private void Start()
    {
        gameObject.SetActive(true);
        if (!IsOwner) return;
        GameObject _UI = GameObject.FindGameObjectWithTag("UI");
        textAmmo = _UI.transform.Find("Ammo").transform.Find("AmmoCount").GetComponent<TextMeshProUGUI>();
        textReserve = _UI.transform.Find("Ammo").transform.Find("ReserveCount").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (timerGun <= 0.0f)
        {
            //can shoot yessir!
            canShoot = true;
        }
        else
        {
            canShoot = false;
            timerGun -= Time.deltaTime;
        }
        //On Everyone ^^^
        if (!IsOwner) return; // anything ran before this line of code is ran on all clients but after its only ran on the person who owns it.
        //On owner VVV
        if (textAmmo != null)
        {
            textAmmo.text = ammo.Value.ToString();
            textReserve.text = reserveAmmo.Value.ToString();
        }   
        if(isAutomatic == true && Input.GetButton("Fire1") && canShoot == true){
            print("attempted tautoo fire weapoin" + weaponID);
            shootServerRPC();
        }     
        else if (Input.GetButtonDown("Fire1") && canShoot == true)
        {
            //fire gun
            print("attempted to fire weapoin" + weaponID);
            shootServerRPC();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //reload
            reloadGunServerRPC();
        }
    }

    [ServerRpc]
    public void shootServerRPC()
    {
        print("bang!!" + weaponID);
        if (ammo.Value >= 1)
        {
            ammo.Value--;
            timerGun = weaponCoolDown;
        }
    }
    [ServerRpc]
    public void reloadGunServerRPC()
    {
        print("reload");
        if (ammo.Value > maxAmmo.Value)
        {
            return;
        }
        else if(canOverfill == false && ammo.Value == maxAmmo.Value)
        {
            return;
        }

        if (maxAmmo.Value == ammo.Value && canOverfill == true && reserveAmmo.Value > 0)  
        {
            ammo.Value += 1;
            reserveAmmo.Value -= 1;
        }
        else
        {
            if (reserveAmmo.Value > 0 && ammo.Value != maxAmmo.Value + 1)
            {
                if (reserveAmmo.Value < maxAmmo.Value)
                {
                    ammo.Value = reserveAmmo.Value;
                    reserveAmmo.Value = 0;
                }
                else
                {
                    reserveAmmo.Value -= maxAmmo.Value - ammo.Value;
                    ammo.Value = maxAmmo.Value;
                }
            }
        }
    }
    /*public string weaponid;
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
        if (!IsOwner) return;
        audSauce = GetComponent<AudioSource>();
        anim = transform.parent.GetComponent<Animation>(); //be careful this is sketchy so is everything below
        GameObject _UI = GameObject.FindGameObjectWithTag("UI");
        ammoCountText = _UI.transform.Find("Ammo").transform.Find("AmmoCount").GetComponent<TextMeshProUGUI>();
        reserveCountText = _UI.transform.Find("Ammo").transform.Find("ReserveCount").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        print("ran");
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
    }*/
}
