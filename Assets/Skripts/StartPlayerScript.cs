using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartPlayerScript : NetworkBehaviour
{

    private gameManagerScript GameManager;
    public void HostButton()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameManagerScript>();
        NetworkManager.Singleton.StartHost();
        GameManager.selectMenu.SetActive(true);
    }
    
    public void ClientButton()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameManagerScript>();
        NetworkManager.Singleton.StartClient();
        GameManager.selectMenu.SetActive(true);
    }

}
