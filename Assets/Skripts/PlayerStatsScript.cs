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
    private TextMeshProUGUI healthActual;
    private int deathCameraInt = 0;
    public TMP_Text healthText;
    public TMP_Text playerIDText;
    public TMP_Text teamIdText;
    public float speeds;
    public string teamName = "noTeam";
    

    //ui creap doign erso quicky speed of light funni stat script

    public Button empButton;
    public Button ceoButton;

    public void takeDamage(int damage)
    {
        //print("taking some damage m8!!!!");
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
        GameObject _ClassUI = GameObject.FindGameObjectWithTag("ClassScreen");
        print(_TeamUI + " timeam ui");
        empButton = _TeamUI.transform.Find("EmployeeButton").GetComponent<Button>();
        ceoButton = _TeamUI.transform.Find("CEOButton").GetComponent<Button>();
        healthActual = _UI.transform.Find("Health").transform.Find("HealthActual").GetComponent<TextMeshProUGUI>();
        if(IsOwner  ){
            print("yes");
        }
        else{
            print("no");
        }
        if(!IsOwner) return;
        empButton.onClick.AddListener(() => switchTeams("emp", _TeamUI));
        ceoButton.onClick.AddListener(() => switchTeams("ceo", _TeamUI));
    }
    
    void switchTeams(string teamSet, GameObject _TeamUI){
        print("is eine running");
        switchServerRPC(teamSet);
        _TeamUI.SetActive(false);
    }
    [ServerRpc(RequireOwnership = false)]
    void switchServerRPC(string teamSet){
        teamName = teamSet;
        switchClientRPC(teamSet);
        Cursor.lockState = CursorLockMode.Locked;
    }
    [ClientRpc]
    void switchClientRPC(string teamSet){
        teamName = teamSet;
    }


  private void Update() {

        teamIdText.text = teamName;


        //timer stuff
        if (playerHealth.Value <= 0 && isPlayerDead.Value == false){
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
        healthActual.text = playerHealth.Value.ToString();
        if(Input.GetKeyDown(KeyCode.K)){
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (!IsOwner) return;
        
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
                switch (teamName)
                {
                    case "ceo":
                        transform.position = GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().ceoRespawnLocations[Random.Range(0, GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().ceoRespawnLocations.Count - 1)].transform.position;  //probably not optimzed or whatever but did i ask?
                        break;
                    case "emp":
                        transform.position = GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().empRespawnLocations[Random.Range(0, GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().empRespawnLocations.Count - 1)].transform.position;  //probably not optimzed or whatever but did i ask?
                        break;
                }
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
        print("ran serverpc t");
        isPlayerDead.Value = true;
        arms.SetActive(false);
        capsule.SetActive(false);
        playerDeathClientRpc();
    }
    [ClientRpc]
    void playerDeathClientRpc(){
        print("ran client t");
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
        arms.SetActive(true);
        capsule.SetActive(true);
    }

}


