using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerStatsScript : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> playerHealth = new NetworkVariable<int>(100);

    public void takeDamage()
    {
        print("taking some damage m8!!!!");
        playerHealth.Value -= 10;
    }
}
