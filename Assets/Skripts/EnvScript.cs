using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class EnvScript : NetworkBehaviour
{
    public List<GameObject> ceoRespawnLocations = new List<GameObject>();
    public List<GameObject> empRespawnLocations = new List<GameObject>();
    [SerializeField] private Transform ceoResOb;
    [SerializeField] private Transform empResOb;


    public void Start()
    {
        foreach(Transform ceo in ceoResOb)
        {
            ceoRespawnLocations.Add(ceo.gameObject);
        }
        foreach (Transform emp in empResOb)
        {
            empRespawnLocations.Add(emp.gameObject);
        }
    }
}
