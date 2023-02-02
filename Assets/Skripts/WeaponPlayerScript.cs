using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlayerScript : MonoBehaviour
{

    public string primary = "nil";
    public string secondary = "nil";

    public int selectedWeapon = 1;
    // Start is called before the first frame update
    void Start()
    {
        primary = "uzi";
        secondary = "colt";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 2;
        }
    }
}
