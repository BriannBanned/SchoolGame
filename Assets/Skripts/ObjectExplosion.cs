using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

public class ObjectExplosion : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> objectHealth = new NetworkVariable<int>(20);
    [SerializeField] private NetworkVariable<int> explosiveDamage = new NetworkVariable<int>(50);
    [SerializeField] private NetworkVariable<int> explosiveRadius = new NetworkVariable<int>(5);
    private GameObject explosive;
    private Rigidbody rigidbodyTemp;

    public void takeDamage(int damage) {
        explosive = transform.gameObject;
        //print("Kaboom?");
        objectHealth.Value -= damage;
        if(objectHealth.Value <= 0 ){
            explosiveDamage.Value = explosiveDamage.Value - objectHealth.Value;
            Explode(explosiveDamage.Value);
            ParticleSystem explodePart = GameObject.Find("ExplosionManager").GetComponent<ParticleSystem>();
            explodePart.transform.position = explosive.transform.position;
            explodePart.Play();
            GameObject.Destroy(explosive);
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
    public void Explode(int explosiveDamage) {
        
        Collider[] explodeColliders = Physics.OverlapSphere(explosive.transform.position, explosiveRadius.Value);
        foreach (var explodeCollider in explodeColliders) {
            if(explodeCollider.transform.GetComponent<Collider>().tag == "Player" || explodeCollider.transform.GetComponent<Collider>().tag == "Destructible" || explodeCollider.transform.GetComponent<Collider>().tag == "Explosive") {
                Vector3 dir = (explosive.transform.position - explodeCollider.transform.position).normalized;
                float dist = Vector3.Distance(explosive.transform.position, explodeCollider.transform.position);

                if(Physics.Raycast(explosive.transform.position, dir, dist - 0.5f) == false) {
                    int disDa = Mathf.FloorToInt(dist);
                    Debug.DrawRay(explosive.transform.position, dir * -dist, Color.red, 10000f);
                    rigidbodyTemp = explodeCollider.transform.GetComponent<Rigidbody>();

                    if(explodeCollider.transform.GetComponent<Collider>().tag == "Player") {
                        explodeCollider.transform.GetComponent<Collider>().GetComponent<PlayerStatsScript>().takeDamage(explosiveDamage - disDa);
                        rigidbodyTemp.AddForce(-dir * 60 * dist / 1.5f);
                    }
                    if(explodeCollider.transform.GetComponent<Collider>().tag == "Destructible") {
                        explodeCollider.transform.GetComponent<Collider>().GetComponent<ObjectStatScript>().takeDamage(explosiveDamage - disDa);
                        rigidbodyTemp.AddForce(-dir * 60 * dist / 1.5f);
                    }
                    if(explodeCollider.transform.GetComponent<Collider>().tag == "Explosive") {
                        explodeCollider.transform.GetComponent<Collider>().GetComponent<ObjectExplosion>().takeDamage(explosiveDamage - disDa);
                        rigidbodyTemp.AddForce(-dir * 60 * dist / 1.5f);
                    }
                }
                }
            }
        }
}

