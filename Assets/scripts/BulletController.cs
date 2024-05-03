using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Collider col;
    Rigidbody rb;
    ParticleSystem particlesystem;
    public GameObject HitImpact;
    void Start()
    {
        col = this.GetComponent<Collider>();
        rb = this.GetComponent<Rigidbody>();
        particlesystem = this.GetComponent<ParticleSystem>();
        HitImpact.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }





    void OnCollisionEnter(Collision collisionInfo)
    {

        rb.isKinematic = true;
        Destroy(particlesystem);
        col.enabled = false;
        HitImpact.SetActive(true);

        if (collisionInfo.gameObject. GetComponent<GlassContainer>() != null)
        {
            collisionInfo.gameObject.GetComponent<GlassContainer>().DamageAmount -- ;
        }

        Destroy(this.gameObject, 1);

    } 

}

