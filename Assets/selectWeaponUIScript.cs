using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class selectWeaponUIScript : NetworkBehaviour
{
    [SerializeField] private WeaponPlayerScript weaponscript;
    [SerializeField] private GameObject weaponsel;

    [SerializeField] private GameObject second;
    [SerializeField] private GameObject primary;

    public void changePlayerWeapon(GameObject button){ //existstsehrer
        if(!IsOwner) return;
        weaponscript.switchWeaponIDServerRPC(button.GetComponent<weaponButtonData>().weaponNumId, button.GetComponent<weaponButtonData>().slot);

    }

    public void switchTab(int tabId){
        if(!IsOwner) return;
        print("switch tab");
        switch(tabId){
            case 1:
            second.SetActive(false);
            primary.SetActive(true);
            break;
            case 2:
            second.SetActive(true);
            primary.SetActive(false);
            break;
        }

    }

    public void closeWeaponUI(){
        if(!IsOwner) return;
        weaponsel.SetActive(false);
        gameObject.GetComponent<PlayerMovement>().isImobile = false;
    }

    void Update(){
        if(!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.M))
        {
            weaponsel.SetActive(true);
            gameObject.GetComponent<PlayerMovement>().isImobile = true;
        }

    }
}
