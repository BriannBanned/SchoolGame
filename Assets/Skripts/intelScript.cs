using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class intelScript : NetworkBehaviour
{

    public Transform carryPoint = null;
    public bool isCarried = false;
    private gameManagerScript GameManager;

    public void Start()
    {
        GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<gameManagerScript>();
    }
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && isCarried == false)
        {
            print("capture intel!");
            carryPoint = col.transform;
            isCarried = true;
            transform.parent = carryPoint;
            transform.localPosition = new Vector3(-0.22f, 1, 1.6f);
            transform.localRotation =  Quaternion.Euler(0, -90, 90);
            startCarryServerRPC(new Vector3(0 , -90,90), new Vector3(0f, 0.077f, -0.551f));
        }

    }


    [ServerRpc(RequireOwnership = false)]
    public void startCarryServerRPC(Vector3 rot, Vector3 pos)
    {
        isCarried = true;
        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(rot);
        startCarryClientRPC(rot, pos);
    }
    [ClientRpc]
    public void startCarryClientRPC(Vector3 rot, Vector3 pos)
    {
        isCarried = true;
        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(rot);
    }




}
