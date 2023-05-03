using System.Runtime.CompilerServices;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class ObjectStatScript : NetworkBehaviour
{
    [SerializeField] public NetworkVariable<int> objectHealth = new NetworkVariable<int>(40);

        public void takeDamage(int damage)
    {
        print("taking some damage m8!!!!");
        objectHealth.Value -= damage;
        if(objectHealth.Value <= 0 ){
            GameObject.Destroy(transform.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
