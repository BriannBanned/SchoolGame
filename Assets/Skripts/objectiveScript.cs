using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class objectiveScript : NetworkBehaviour
{
    [SerializeField] private GameObject papers;
    public string teamName;// what can capture this
    public float timer;
    [SerializeField] private Transform target;
    public float timerLength = 10;
    private gameManagerScript gameManager;
    [SerializeField] private ParticleSystem particles;
    private void Start()
    {
        papers.SetActive(false);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameManagerScript>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<intelScript>()){
            if(other.transform.parent.gameObject.GetComponent<PlayerStatsScript>().teamName == teamName){
                //start the shredding countdown or whatever
                gameManager.isShredderShredding = true;
                startShredderServerRPC();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void startShredderServerRPC()
    {
        gameManager.isShredderShredding = true;
        Destroy(gameManager.empPapers.gameObject); // gotta keep these syned n wat not
        startShredderClientRPC();
    }
    [ClientRpc]
    public void startShredderClientRPC()
    {
        gameManager.isShredderShredding = true;
        Destroy(gameManager.empPapers.gameObject);
    }


    void Update() 
     {
        transform.GetComponent<Animator>().SetBool("isShredding", gameManager.isShredderShredding);
        papers.GetComponent<Animator>().SetBool("isShredding", gameManager.isShredderShredding);
        papers.SetActive(gameManager.isShredderShredding);
        if (gameManager.isShredderShredding)
        {
            if (!particles.isPlaying) particles.Play();
            timer += Time.deltaTime;
            if(timer >= timerLength)
            {
                gameManager.isShredderShredding = false;
                print("ppftfffffffff");
            }
        }
        else
        {

            if (particles.isPlaying) particles.Stop();
        }
     }









}


