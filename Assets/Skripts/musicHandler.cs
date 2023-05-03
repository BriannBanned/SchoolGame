using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class musicHandler : NetworkBehaviour
{
    // Start is called before the first frame update

    private gameManagerScript GameManager;
    private AudioSource audSource;

//list of music
[SerializeField] private AudioClip shredderMusic;

    void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameManagerScript>();
        audSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        //ok so this is the priority list.
        if (GameManager.isShredderShredding && audSource.clip != shredderMusic) 
        {
            audSource.Stop();
            audSource.clip = shredderMusic;
            audSource.Play();
            return; //make sure to return so the others dont get played
        }

        if (audSource.isPlaying) return; //anything below will play after current music anything above should override. (just realized this all might be useless not sure what more music we would add.

    }
}
