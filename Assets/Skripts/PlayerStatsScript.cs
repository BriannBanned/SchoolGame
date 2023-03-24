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
    public string teamName = "noTeam";

    //ui creap doign erso quicky speed of light

    public Button empButton;
    public Button ceoButton;

    public void takeDamage(int damage)
    {
        print("taking some damage m8!!!!");
        playerHealth.Value -= damage;
        if(playerHealth.Value < 0 ){
            playerHealth.Value = 0;
        }
    }

    private void Start()
    {
        playerIDText.text = gameObject.GetComponent<NetworkObject>().NetworkObjectId.ToString();

        GameObject _UI = GameObject.FindGameObjectWithTag("UI");
        GameObject _TeamUI = GameObject.FindGameObjectWithTag("TeamUI");
        ceoButton = _TeamUI.transform.Find("CEOButton").GetComponent<Button>();
        empButton = _TeamUI.transform.Find("EmployeeButton").GetComponent<Button>();
        if(IsOwner  ){
            print("yes");
        }
        else{
            print("no");
        }
        if(!IsOwner) return;
        empButton.onClick.AddListener(() => switchTeams("emp"));
        ceoButton.onClick.AddListener(() => switchTeams("ceo"));
    }

    
    void switchTeams(string teamSet){
        print("is eine running");
        switchServerRPC(teamSet);
    }
    [ServerRpc(RequireOwnership = false)]
    void switchServerRPC(string teamSet){
        teamName = teamSet;
        switchClientRPC(teamSet);
    }
    [ClientRpc]
    void switchClientRPC(string teamSet){
        teamName = teamSet;
    }

  private void Update() {

        healthText.text = playerHealth.Value.ToString();
        if(Input.GetKeyDown(KeyCode.K)){
            Cursor.lockState = CursorLockMode.None;
        }

    }
}
