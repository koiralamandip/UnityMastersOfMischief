using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatHitBox : MonoBehaviour
{

    [Header("Damage")]

    [SerializeField]  private float KickPower;

    public List<GameObject> AffectedObjects;

    Collider BoxCollider;


    [Header("Shake and Particle")]

    [SerializeField] private CameraFollow CameraScript;
    [SerializeField] private ParticleSystem HitParticle;
  
    void Start()
    {
        BoxCollider = this.GetComponent<Collider>();
    }



    // Update is called once per frame
    void Update()
    {
       if(BoxCollider.enabled == false)
        {
             

            AffectedObjects = new List<GameObject>();
        }
        
    }
    void FixedUpdate()
    {
        for (int I = 0; I < AffectedObjects.Count; I++)
        {
         
            AffectedObjects[I].GetComponent<Rigidbody>().AddForce(transform.up * KickPower, ForceMode.Impulse);
          
             
            

        }
    }




    /*
    void OnTriggerEnter(Collider collidee)
    {
        if(collidee.GetComponent<Rigidbody>() != null && BoxCollider.enabled == true)
        {
            AffectedObjects.Add(collidee.gameObject);
             

            HitParticle.Play();
             
            HitParticle.transform.parent = null;
        }



    }


    void OnTriggerExit(Collider collidee)
    {
        AffectedObjects.Remove(collidee.gameObject);
    }
    */

    void OnCollisionEnter(Collision collision)
    {

       


        foreach (ContactPoint contact  in collision.contacts)
        {

            /////////////////////////////////////////////////////////////
           

            if (collision.gameObject.GetComponent<Rigidbody>() != null)
            {

                Vector3 position = contact.point;
                // Instantiate(explosionPrefab, position, rotation);

                HitParticle.Play();

                HitParticle.transform.position = position;
                CameraScript.shaketrue = true;



                 Vector3 dir = contact.point - transform.position;
                  dir = dir.normalized;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * KickPower, ForceMode.Impulse);
              //  collision.rigidbody.AddForce( contact.normal * KickPower, ForceMode.Impulse);



            }




            ///////////////////////////////// hiting the glass 
            if (collision.gameObject.CompareTag("glass"))
            {
                Vector3 position = contact.point;
                // Instantiate(explosionPrefab, position, rotation);

                collision.gameObject.GetComponent<GlassContainer>().DamageAmount--;
                
                HitParticle.Play();

                HitParticle.transform.position = position;
                CameraScript.shaketrue = true;

            }
        }

    }


   }
