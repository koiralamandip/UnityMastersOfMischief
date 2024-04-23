using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CombatWeaponRotation : MonoBehaviour
{
    
    [Header("Movemnets")]

    [SerializeField] private float speed = 5f;
    //adjust this to change how high it goes
    [SerializeField] private float height = 0.5f;
    Vector3 intialPosition;


    [SerializeField] private float RotateSpeed = 5f;

    [Header("Appearance")]

    [SerializeField] private Outline OutlineScript ;

    [Header("Type of weapon")]
    [SerializeField] private bool WoodBat ;
    [SerializeField] private bool KriketBat;
    [SerializeField] private bool PoliceBat;
    [SerializeField] private bool Bone;
    [SerializeField] private bool Lolipop;
    [SerializeField] private bool ChikenLeg;
    [SerializeField] private bool Mallet;
    [SerializeField] private bool Plunger;
    [SerializeField] private bool PolicelBat;
    [SerializeField] private bool Shovel;
    void Start()
    {
       // transform.eulerAngles = new Vector3(0, 0, 90);
        intialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        ////////////////////////////////////// up and down movment and rotate


        transform.position = new Vector3(intialPosition.x, Mathf.Sin(Time.time * speed) * height + intialPosition.y, intialPosition.z);

         

        transform.Rotate(0, Time.deltaTime * RotateSpeed, 0);


        
    }



    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            
             

            OutlineScript.OutlineWidth = 10;
            
            //  other.gameObject.GetComponent<Bear>().coinNumber++;
            // coinCounter.instanceCoin.IncreaseCoin(+1);

            // Destroy(this.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            OutlineScript.OutlineWidth = 0;
            
            //  other.gameObject.GetComponent<Bear>().coinNumber++;
            // coinCounter.instanceCoin.IncreaseCoin(+1);

            // Destroy(this.gameObject);
        }
    }


    void OnTriggerStay(Collider other)
    {

        if (WoodBat && Input.GetKeyDown(KeyCode.E))
        {
            if (other.GetComponent<RatPlayer>() != null)
            {
                other.GetComponent<RatPlayer>().WoodBat = true;
                Destroy(this.gameObject);
            }

            if (other.GetComponent<CatPlayer>() != null)
            {
                
            }

        }



        if (KriketBat)
        {
            other.GetComponent<RatPlayer>().KriketBat = true;
        }






    }

}
