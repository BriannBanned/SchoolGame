using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartPlayerScript : NetworkBehaviour
{
    public void HostButton()
    {
        NetworkManager.Singleton.StartHost();
    }
    
    public void ClientButton()
    {
        NetworkManager.Singleton.StartClient();
    }

}
