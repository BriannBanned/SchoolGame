using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectiveScript : MonoBehaviour
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
            if(other.transform.parent.parent.gameObject.GetComponent<PlayerStatsScript>().teamName == teamName){
                //start the shredding countdown or whatever
                gameManager.isShredderShredding = true;
                Destroy(other.gameObject);
            }
        }
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


