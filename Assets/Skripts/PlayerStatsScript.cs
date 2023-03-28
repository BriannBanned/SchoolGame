using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class PlayerStatsScript : NetworkBehaviour
{
    [Header("stufnt")]
    [SerializeField] private GameObject arms;
    [SerializeField] private GameObject capsule;
    [SerializeField] private GameObject camera;
    [SerializeField] public NetworkVariable<int> playerHealth = new NetworkVariable<int>(100);
    [SerializeField] public NetworkVariable<bool> isPlayerDead = new NetworkVariable<bool>(false);
    [SerializeField] private float respawnTimer;
    private float respawnPrivate;
    [SerializeField] private float cameraSwitchTimer;
    private float  cameraPrivate;

    private List<GameObject> deathCameras = new List<GameObject>();
    private int deathCameraInt = 0;
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




    //timer stuff
        if(playerHealth.Value <= 0 && isPlayerDead.Value == false){
            playerDeathServerRpc();
            if (IsOwner)
            {
                camera.SetActive(false);
                deathCameras.Clear();
                deathCameraInt = 0;
                respawnPrivate = respawnTimer;
                Transform t = GameObject.FindGameObjectWithTag("Env").transform.Find("DeathCameras");
                foreach(Transform child in t)
                {
                    deathCameras.Add(child.gameObject);
                }
            }
        }
        healthText.text = playerHealth.Value.ToString();
        if(Input.GetKeyDown(KeyCode.K)){
            takeDamage(10);
        }
        if(!IsOwner) return;
        
        if(isPlayerDead.Value){
            cameraPrivate -= 1f * Time.deltaTime;
            respawnPrivate -= 1f * Time.deltaTime;
            
            if(cameraPrivate <= 0){
                cameraPrivate = cameraSwitchTimer;
                switchCameraDeath();
            }
            if(respawnPrivate <= 0){
                print("undead");
                deathCameras[deathCameraInt].SetActive(false);
                camera.SetActive(true);
                playerUnDeathServerRpc();
                print(isPlayerDead.Value);
            }
      }

    }

    void switchCameraDeath()
    {
        print(deathCameraInt);
        deathCameras[deathCameraInt].SetActive(false);
        deathCameraInt++;
        if(deathCameraInt > deathCameras.Count -1){
            deathCameraInt = 0;
        }
        deathCameras[deathCameraInt].SetActive(true);
    }

    [ServerRpc]
    void playerDeathServerRpc()
    {
        isPlayerDead.Value = true;
        arms.SetActive(false);
        capsule.SetActive(false);
        playerDeathClientRpc();
    }
    [ClientRpc]
    void playerDeathClientRpc(){
        isPlayerDead.Value = true;
        arms.SetActive(false);
        capsule.SetActive(false);
    }

    [ServerRpc]
    void playerUnDeathServerRpc(){
        isPlayerDead.Value = false;
        playerHealth.Value = 100;
        arms.SetActive(true);
        capsule.SetActive(true);
        playerUnDeathClientRpc();
    }

    [ClientRpc]
    void playerUnDeathClientRpc(){
        isPlayerDead.Value = false;
        arms.SetActive(true);
        capsule.SetActive(true);
    }

}


