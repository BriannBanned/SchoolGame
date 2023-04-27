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
    [SerializeField] private NetworkVariable<int> explosiveRadius = new NetworkVariable<int>(15);
    private GameObject explosive;
    private Rigidbody rigidbodyTemp;

    public void takeDamage(int damage) {
        explosive = transform.gameObject;
        //print("Kaboom?");
        objectHealth.Value -= damage;
        if(objectHealth.Value <= 0 ){
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
            if(explodeCollider.gameObject.tag == "Player" || explodeCollider.gameObject.tag == "Destructible" || explodeCollider.gameObject.tag == "Explosive") {
                print("Explosive Hit: " + explodeCollider);
                Vector3 dir = (explosive.transform.position - explodeCollider.transform.position).normalized;
                float dist = Vector3.Distance(explosive.transform.position, explodeCollider.transform.position);

                if(Physics.Raycast(explosive.transform.position, -dir, dist - 1f) == false) {
                    int disDa = Mathf.RoundToInt(dist);
                    rigidbodyTemp = explodeCollider.transform.GetComponent<Rigidbody>();
                    print(explosiveDamage + " " + disDa);

                    if(explodeCollider.transform.GetComponent<Collider>().tag == "Player") {
                        explodeCollider.transform.GetComponent<Collider>().GetComponent<PlayerStatsScript>().takeDamage(explosiveDamage - disDa);
                        rigidbodyTemp.AddForce(-dir * 60f * dist / 1.5f);
                    }
                    if(explodeCollider.transform.GetComponent<Collider>().tag == "Destructible") {
                        explodeCollider.transform.GetComponent<Collider>().GetComponent<ObjectStatScript>().takeDamage(explosiveDamage - disDa);
                        rigidbodyTemp.AddForce(-dir * 60f * dist / 1.5f);
                    }
                    if(explodeCollider.transform.GetComponent<Collider>().tag == "Explosive" && explodeCollider.gameObject != explosive) {
                        explodeCollider.transform.GetComponent<Collider>().GetComponent<ObjectExplosion>().takeDamage(explosiveDamage - disDa);
                        rigidbodyTemp.AddForce(-dir * 60f * dist / 1.5f);
                        
                    }
                }
                }
            }
        }
}

