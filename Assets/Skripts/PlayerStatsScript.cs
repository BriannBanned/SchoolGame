using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class PlayerStatsScript : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> playerHealth = new NetworkVariable<int>(100);
    public TMP_Text healthText;
    public TMP_Text playerIDText;

    public void takeDamage(int damage)
    {
        print("taking some damage m8!!!!");
        playerHealth.Value -= damage;
        if(playerHealth.Value < 0 ){
            playerHealth.Value = 0;
        }
    }

    private void Awake()
    {
        playerIDText.text = gameObject.GetComponent<NetworkObject>().NetworkObjectId.ToString();

    }
    private void Update(){
                healthText.text = playerHealth.Value.ToString();
    }
}
