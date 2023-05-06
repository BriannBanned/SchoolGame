using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponPlayerScript : NetworkBehaviour
{

    public string primary = "nil";
    public string secondary = "nil";
    public GameObject primaryObject = null;
    public GameObject secondaryObject = null;
    public GameObject Arms;
    public int selectedWeapon = 2;
    private gameManagerScript GameManager; //ily game manager



    public GameObject[] weapons = new GameObject[] { };
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameManagerScript>(); // thats all i needed to bus
        selectedWeapon = 1;
        primary = "ak74";
        secondary = "Uzi";
        secondaryObject = Arms.transform.Find(secondary.ToString()).gameObject;
        primaryObject = Arms.transform.Find(primary.ToString()).gameObject;

        foreach(GameObject wep in weapons)
        {
            wep.SetActive(false);
        }
        
        switch(selectedWeapon){
            case 1:
            secondaryObject.SetActive(false);
            primaryObject.SetActive(true);
            break;

            case 2:
            primaryObject.SetActive(false);
            secondaryObject.SetActive(true);
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.Alpha1) && primary != "null")
        {
            switchPlayerWeaponServerRPC(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && secondary != "null")
        {
            switchPlayerWeaponServerRPC(2);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            switchWeapon(1,"ak47");
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void switchWeaponIDServerRPC(int weaponNumID, int slot){

        switch (slot)
        {

            case 1:
                //primary
                primaryObject.SetActive(false);
                primary = GameManager.weaponNums[weaponNumID];
                secondaryObject = Arms.transform.Find(secondary.ToString()).gameObject;
                primaryObject = Arms.transform.Find(primary.ToString()).gameObject;
                break;
            case 2:
                //secondary
                secondaryObject.SetActive(false);
                secondary = GameManager.weaponNums[weaponNumID];
                secondaryObject = Arms.transform.Find(secondary.ToString()).gameObject;
                primaryObject = Arms.transform.Find(primary.ToString()).gameObject;
                break;
        }
        switch (selectedWeapon)
        {
            case 1:
                primaryObject.SetActive(true);
                break;

            case 2:
                secondaryObject.SetActive(true);
                break;
        }
        secondaryObject = Arms.transform.Find(secondary.ToString()).gameObject;
        primaryObject = Arms.transform.Find(primary.ToString()).gameObject;
        switchWeaponIDClientRPC(weaponNumID, slot);
    }
    [ClientRpc]
    public void switchWeaponIDClientRPC(int weaponNumID, int slot){

        switch(slot){

            case 1:
            //primary
            primaryObject.SetActive(false);
            primary = GameManager.weaponNums[weaponNumID];
            secondaryObject = Arms.transform.Find(secondary.ToString()).gameObject;
            primaryObject = Arms.transform.Find(primary.ToString()).gameObject;
            break;
            case 2:
            //secondary
            secondaryObject.SetActive(false);
            secondary = GameManager.weaponNums[weaponNumID];
            secondaryObject = Arms.transform.Find(secondary.ToString()).gameObject;
            primaryObject = Arms.transform.Find(primary.ToString()).gameObject;
            break;
        }
        switch(selectedWeapon){
            case 1:
            primaryObject.SetActive(true);
            break;

            case 2:
            secondaryObject.SetActive(true);
            break;
        }
        secondaryObject = Arms.transform.Find(secondary.ToString()).gameObject;
        primaryObject = Arms.transform.Find(primary.ToString()).gameObject;
    }
    [ServerRpc]
    public void switchPlayerWeaponServerRPC(int slot)
    {
        switch (slot)
        {
            case 1:
                secondaryObject.SetActive(false);
                primaryObject.SetActive(true);
                selectedWeapon = 1;
                break;
            case 2:
                secondaryObject.SetActive(true);
                primaryObject.SetActive(false);
                selectedWeapon = 2;
                break;
        }
        switchPlayerWeaponClientRPC(slot);
    }

    [ClientRpc]
    public void switchPlayerWeaponClientRPC(int slot)
    {
        switch (slot)
        {
            case 1:
                secondaryObject.SetActive(false);
                primaryObject.SetActive(true);
                selectedWeapon = 1;
                break;
            case 2:
                secondaryObject.SetActive(true);
                primaryObject.SetActive(false);
                selectedWeapon = 2;
                break;
        }
    }
    public void switchWeapon(int whichWeapon, string toWhat)
    {
        switch (whichWeapon)
        {
            case 1:
                primary = toWhat;
                if (selectedWeapon == 1)
                {
                    putAwayWeapon(1);
                }
                primaryObject = Arms.transform.Find(toWhat).gameObject;
                if (selectedWeapon == 1)
                {
                    bringOutWeapon(1);
                }
                break;
            case 2:
                secondary = toWhat;
                if (selectedWeapon == 2)
                {
                    putAwayWeapon(2);
                }
                secondaryObject = Arms.transform.Find(toWhat).gameObject;
                if (selectedWeapon == 2)
                {
                    bringOutWeapon(2);
                }
                break;
        }
        {
            
        }
    }
    
    public void putAwayWeapon(int weapon) // use both of these because put away animations and bring out animations might exist soon idk
    {
        switch (weapon)
        {
            case 1:
                primaryObject.SetActive(false);
                break;
            case 2:
                secondaryObject.SetActive(false);
                break;
        }
    }

    public void bringOutWeapon(int weapon)
    {
        switch (weapon)
        {
            case 1:
                primaryObject.SetActive(true);
                break;
            case 2:
                secondaryObject.SetActive(true);
                break;
        }
    }
}
