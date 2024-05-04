using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassContainer : MonoBehaviour
{
    [SerializeField] public float DamageAmount = 5f;
    [SerializeField] private Transform BrokenGlass  ;
    [SerializeField] private Transform UnbrokenGlass;
    [SerializeField] private Transform TreasureObject;
    public List<Rigidbody> AffectedObjects =new List<Rigidbody>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(DamageAmount<= 3)
        {
            BrokenGlass.gameObject.SetActive(true);
            UnbrokenGlass.gameObject.SetActive(false);

        }
        if (DamageAmount <= 0)
        {

            this.GetComponent<Collider>().enabled = false;
            TreasureObject.GetComponent<BoxCollider>().enabled = true;
            TreasureObject.GetComponent<Rigidbody>().isKinematic = false;
            for (int I = 0; I < AffectedObjects.Count; I++)
            {

                AffectedObjects[I].GetComponent<Rigidbody>().isKinematic =false;




            }


        }
    }

    void OnCollisionEnter(Collision collision)
    {




        foreach (ContactPoint contact in collision.contacts)
        {

            


            if (collision.gameObject.GetComponent<Rigidbody>() != null)
            {


            }

        }

    }
 }
