using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intelScript : MonoBehaviour
{

    public Transform carryPoint = null;
    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player"){
            print("capture intel!");
            Transform carryPoint = col.transform.Find("CarryPoint");
            transform.parent = carryPoint;
            transform.position = carryPoint.position;
            transform.rotation = carryPoint.rotation;
        }

    }
}
