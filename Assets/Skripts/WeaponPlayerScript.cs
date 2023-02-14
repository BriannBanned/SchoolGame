using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayerScript : MonoBehaviour
{

    public string primary = "nil";
    public string secondary = "nil";
    public GameObject primaryObject = null;
    public GameObject secondaryObject = null;
    public GameObject Arms;
    public int selectedWeapon = 1;

    public GameObject[] weapons = new GameObject[] { };
    // Start is called before the first frame update
    void Start()
    {
        selectedWeapon = 1;
        primary = "m48";
        secondary = "colt";
        secondaryObject = Arms.transform.Find("colt").gameObject;
        primaryObject = Arms.transform.Find("m48").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && primary != "null")
        {
            selectedWeapon = 1;
            secondaryObject.SetActive(false);
            primaryObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && secondary != "null")
        {
            selectedWeapon = 2;
            secondaryObject.SetActive(true);
            primaryObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            switchWeapon(1,"colt");
        }
    }

    public void switchWeapon(int whichWeapon, string toWhat)
    {
        switch (whichWeapon)
        {
            case 1:
                primary = toWhat;
                instWeapon(toWhat, whichWeapon);
                break;
            case 2:
                secondary = toWhat;
                instWeapon(toWhat, whichWeapon);
                break;
        }
        {
            
        }
    }

    public void instWeapon(string toWhat, int which)
    {
        GameObject _instPrefab = null;
        print("GOING TO RUN THE LOOP NOW");
        for (int i = 0; i < weapons.Length; i++)
        {
            print("runnin loop");
            print(i);
            print(weapons[i].name);
            if (weapons[i].name == toWhat)
            {
                _instPrefab = weapons[i];
                print("set now");
                break;
            }
        }
        print("isnt");
        GameObject newWeapon =  Instantiate(_instPrefab, Arms.transform.position, Quaternion.Euler(Arms.transform.parent.eulerAngles.x,-180,Arms.transform.parent.eulerAngles.z), Arms.transform);
        newWeapon.transform.localPosition += newWeapon.GetComponent<weaponscript>().spawnOffset;
        switch (which)
        {
            case 1:
                Destroy(primaryObject);
                primary = toWhat;
                primaryObject = newWeapon;
                break;
            case 2:
                Destroy(secondaryObject);
                secondary = toWhat;
                secondaryObject = newWeapon;
                break;
        }
    }
}
