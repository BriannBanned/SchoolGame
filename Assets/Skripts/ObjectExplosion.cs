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
    [SerializeField] private NetworkVariable<int> explosiveRadius = new NetworkVariable<int>(10);
    private GameObject explosive;
    private Rigidbody rigidbodyTemp;
    private bool hasExploded = false;

    public void takeDamage(int damage) {
        explosive = transform.gameObject;
        //print("Kaboom?");
        objectHealth.Value -= damage;
        if (objectHealth.Value < 0) {
            objectHealth.Value = 0;
        }
        if(objectHealth.Value == 0 ){
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

        if(hasExploded == true) {
            return;
        }
        hasExploded = true;

        Collider[] explodeColliders = Physics.OverlapSphere(explosive.transform.position, explosiveRadius.Value);
        foreach (var explodeCollider in explodeColliders) {

            if(explodeCollider.gameObject.tag == "Player" || explodeCollider.gameObject.tag == "Destructible" || explodeCollider.gameObject.tag == "Explosive") {
                Vector3 dir = (explosive.transform.position - explodeCollider.transform.position).normalized;
                float dist = Vector3.Distance(explosive.transform.position, explodeCollider.transform.position);

                if(Physics.Raycast(explosive.transform.position, -dir, out RaycastHit hitEx, dist + 0.2f)) {
                int disDa = Mathf.RoundToInt(dist);
                rigidbodyTemp = explodeCollider.transform.GetComponent<Rigidbody>();
                print(hitEx.collider);

                    if(hitEx.collider.tag == "Player") {
                        explodeCollider.transform.GetComponent<Collider>().GetComponent<PlayerStatsScript>().takeDamage(explosiveDamage - disDa);
                        rigidbodyTemp.AddForce(-dir * 60f * dist / 1.5f);
                    }
                    if(hitEx.collider.tag == "Destructible") {
                        explodeCollider.transform.GetComponent<Collider>().GetComponent<ObjectStatScript>().takeDamage(explosiveDamage - disDa);
                        rigidbodyTemp.AddForce(-dir * 60f * dist / 1.5f);
                    }
                    if(hitEx.collider.tag == "Explosive" && explodeCollider.gameObject != explosive) {
                        explodeCollider.transform.GetComponent<Collider>().GetComponent<ObjectExplosion>().takeDamage(explosiveDamage - disDa);
                        rigidbodyTemp.AddForce(-dir * 60f * dist / 1.5f); 
                    }
                }
            }
        }
    }
        
}

