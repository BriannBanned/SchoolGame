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
    [SerializeField] public NetworkVariable<float> playerHealth = new NetworkVariable<float>(100);
    public float maxPlayerHealth = 100f;
    [SerializeField] public NetworkVariable<bool> isPlayerDead = new NetworkVariable<bool>(false);
    [SerializeField] private float respawnTimer;
    private float respawnPrivate;
    [SerializeField] private float cameraSwitchTimer;
    private float  cameraPrivate;
    public NetworkVariable<int> playerClass = new NetworkVariable<int>(0);

    private List<GameObject> deathCameras = new List<GameObject>();
    private int deathCameraInt = 0;
    public TMP_Text healthText;
    public TMP_Text playerIDText;
    public TMP_Text teamIdText;
    public string teamName = "noTeam";
    //clas vars
    private gameManagerScript GameManager;

    //disableenable
    
    public void flipCharacter(bool flip){
        arms.SetActive(flip);
        capsule.SetActive(flip);
    }
    

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

        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameManagerScript>(); //class thingy
        playerIDText.text = gameObject.GetComponent<NetworkObject>().NetworkObjectId.ToString();

        GameObject _UI = GameObject.FindGameObjectWithTag("UI");
        GameObject _TeamUI = GameObject.FindGameObjectWithTag("TeamUI");
        print(_TeamUI + " timeam ui");
        empButton = _TeamUI.transform.Find("EmployeeButton").GetComponent<Button>();
        ceoButton = _TeamUI.transform.Find("CEOButton").GetComponent<Button>();

        if(!IsOwner) return;
        flipCharacter(false);
        hideRevealClass(true);
        camera.SetActive(false);
        GameManager.crossHair.SetActive(false);
        empButton.onClick.AddListener(() => switchTeams("emp"));
        ceoButton.onClick.AddListener(() => switchTeams("ceo"));
        GameManager.lightButton.GetComponent<Button>().onClick.AddListener(() => selectClass(1));
        GameManager.heavyButton.GetComponent<Button>().onClick.AddListener(() => selectClass(3));
        GameManager.balancedButton.GetComponent<Button>().onClick.AddListener(() => selectClass(2));
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

    [ServerRpc]
    private void updateDataServerRPC()
    {

    }
    [ClientRpc]
    private void updateDataClientRPC()
    {

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
        healthText.text = playerHealth.Value.ToString();
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
        flipCharacter(false);
        playerDeathClientRpc();
    }
    [ClientRpc]
    void playerDeathClientRpc(){
        print("ran client t");
        flipCharacter(false);
    }

    [ServerRpc]
    void playerUnDeathServerRpc(){
        isPlayerDead.Value = false;
        playerHealth.Value = maxPlayerHealth;
        flipCharacter(true);
        playerUnDeathClientRpc();
        
    }

    [ClientRpc]
    void playerUnDeathClientRpc(){
        flipCharacter(true);
    }

    // class stuff
    void hideRevealClass(bool theBool){

        if(theBool){
            GameManager.crossHair.SetActive(false);
            GameManager.selectCamera.SetActive(true);
            GameManager.selectMenu.SetActive(true);
        }
        else{
            GameManager.crossHair.SetActive(true);
            GameManager.selectCamera.SetActive(false);
            GameManager.selectMenu.SetActive(false);
        }

    }


    public void selectClass(int classType)
    {
        flipCharacter(true);
        selectClassServerRPC(classType);
        gameObject.GetComponent<CharacterController>().enabled = false; //cant teleport the player without this schiesse
        switch (teamName)
        {
            case "ceo":
                print("ceo");
                transform.position = GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().ceoRespawnLocations[Random.Range(0, GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().ceoRespawnLocations.Count - 1)].transform.position;  //probably not optimzed or whatever but did i ask?
                break;
            case "emp":
                transform.position = GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().empRespawnLocations[Random.Range(0, GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().empRespawnLocations.Count - 1)].transform.position;  //probably not optimzed or whatever but did i ask?
                break;
        }
        gameObject.GetComponent<CharacterController>().enabled = true; //cant teleport the player without this schiesse
        gameObject.GetComponent<PlayerMovement>().isImobile = false;
        if (!IsOwner) return;
        Cursor.lockState = CursorLockMode.Locked;
        camera.SetActive(true);
        GameManager.crossHair.SetActive(true);
        GameManager.selectCamera.SetActive(false);
        GameManager.selectMenu.SetActive(false);

    }
    [ServerRpc]
    public void selectClassServerRPC(int theClass)
    {
        playerClass.Value = theClass;
        //selected the clsas so set to a spawn or whatever.
        gameObject.GetComponent<CharacterController>().enabled = false; //cant teleport the player without this schiesse
        switch (teamName)
        {
            case "ceo":
                print("ceo");
                transform.position = GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().ceoRespawnLocations[Random.Range(0, GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().ceoRespawnLocations.Count - 1)].transform.position;  //probably not optimzed or whatever but did i ask?
                break;
            case "emp":
                transform.position = GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().empRespawnLocations[Random.Range(0, GameObject.FindGameObjectWithTag("Env").GetComponent<EnvScript>().empRespawnLocations.Count - 1)].transform.position;  //probably not optimzed or whatever but did i ask?
                break;
        }
        gameObject.GetComponent<CharacterController>().enabled = true;//cant teleport the player without this schiesse
        flipCharacter(true);
        gameObject.GetComponent<PlayerMovement>().isImobile = false;
        switch(theClass){
            case 1:
                maxPlayerHealth = 75;
                break;
            case 2:
                maxPlayerHealth = 100; //need to change these in playermovement as well )for the time being)
                break;
            case 3:
                maxPlayerHealth = 200;
                break;
        }
        playerHealth.Value = maxPlayerHealth;
        print(playerHealth.Value);

    } 
}



