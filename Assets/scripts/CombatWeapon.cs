using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CombatWeapon : MonoBehaviour
{
    
    [Header("Movemnets Behaviour")]

    [SerializeField] private float speed = 5f;
    
    [SerializeField] private float height = 0.5f;
     //  Vector3 intialPosition;
    
   
    [SerializeField] private float RotateSpeed = 5f;



    [SerializeField] private float dropForwardForce = 5f;
    [SerializeField] private float dropUpwardForce = 5f;

    [SerializeField] private Outline OutlineScript;
    [SerializeField] public bool IsPicked;
    [SerializeField] private bool IsDrop;
    [SerializeField] private Collider Collider;
    Collider triggerCollider;
    Rigidbody RB;
    Quaternion startQuaternion;
    float YPosition;
    float vel;
    Vector3 intialPosition;
      float timer;

   
   


    [Header("Type of weapon")]
    [SerializeField] private bool IsCombat ;
    [SerializeField] private bool IsBoomrang;
    [SerializeField] private bool IsGun;
    [SerializeField] private Transform CombatWeaponContainer;

    PlayerController playerScript;
    void Start()
    {
        // transform.eulerAngles = new Vector3(0, 0, 90);
        // intialPosition = transform.position;
        triggerCollider = this.GetComponent<Collider>();

        RB = this.GetComponent<Rigidbody>();

        startQuaternion = transform.rotation;

        intialPosition = transform.position;
    }

    void LateUpdate()
    {

      

    }



    private void FixedUpdate()
    {
         
       
    }
    void Update()
    {


       



        /////////////////////
        vel = RB.velocity.y;

        if (RB.velocity.y < -22)
        {
            transform.rotation = startQuaternion;
            transform.position= intialPosition  ;
            RB.velocity = Vector3.zero;
        }

        timer = Mathf.Clamp(timer,0, 1.2f);
        if ( Input.GetKeyDown(KeyCode.E) && CombatWeaponContainer != null && timer ==0)

        {

            IsPicked = !IsPicked;
             

        }

        ////////////////////////////////////// up and down movment and rotate


        if (IsPicked && IsDrop==false)
        {
            timer += Time.deltaTime;
            
            Collider.enabled = false;
            //  transform.localPosition = Vector3.MoveTowards(transform.localPosition, TargetPositionAndRotaion.localPosition, 12 *Time.deltaTime);
            triggerCollider.enabled = false;

            RB.isKinematic = true;
            OutlineScript.OutlineWidth = 0;


          

            transform.SetParent(CombatWeaponContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            transform.localScale = Vector3.one;



            if (IsCombat && playerScript != null)
            {
                playerScript.HasCombatWeapon = true;

                playerScript.HasBoomrang = false;
                playerScript.HasGunWeapon = false;

            }



            /////////// when the boomrang is picked and ready to shot
            if (IsBoomrang && playerScript != null)
            {
                playerScript.HasCombatWeapon = false;

                playerScript.HasBoomrang = true;
                playerScript.HasGunWeapon = false;
            }

            /////////// when the boomrang is picked and ready to shot
            if (IsGun && playerScript != null)
            {
                playerScript.HasCombatWeapon = false;

                playerScript.HasBoomrang = false;
                playerScript.HasGunWeapon = true;
            }




        }
        



        if ( IsPicked==false && IsDrop ==false)
        {
            RB.isKinematic = true;
            Collider.enabled = false;
            transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * speed) * height + transform.position.y, transform.position.z);
            transform.Rotate(0, Time.deltaTime * RotateSpeed, 0);
          
            triggerCollider.enabled = true;


            if (  playerScript!= null )
            {
                playerScript.HasCombatWeapon = false;

                playerScript.HasBoomrang = false;
                playerScript.HasGunWeapon = false;
            }
 







        }



        /////////////////////////////////////////////////////
        if (timer < 1 && IsPicked)
            return;
        if (timer>1f && Input.GetKeyDown(KeyCode.E) && RB.isKinematic == true)

        {

            IsDrop = true;
            CombatWeaponContainer = null;
            OutlineScript.OutlineWidth =  0;
            RB.isKinematic = false;
            transform.parent = null;
            Collider.enabled = true;
            triggerCollider.enabled = true;
            //AddForce
            RB.AddForce(transform.forward * dropForwardForce, ForceMode.Impulse);
            RB.AddForce(Vector3.up * dropUpwardForce, ForceMode.Impulse);
            //Add random rotation
            float random = Random.Range(-10f, 10f);
            RB.AddTorque(new Vector3(random, random, random) * 50);


        } 


        



    }



        void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player") )
        {

            
            if(IsCombat || IsBoomrang)
            {

              OutlineScript.OutlineWidth = 10;
              CombatWeaponContainer = other.GetComponent<PlayerController>().CombatWeaponContainer;
               playerScript =  other.GetComponent<PlayerController>();
                //  other.gameObject.GetComponent<Bear>().coinNumber++;
                // coinCounter.instanceCoin.IncreaseCoin(+1);

                // Destroy(this.gameObject);
            }
            if (IsGun)
            {
                OutlineScript.OutlineWidth = 10;
                CombatWeaponContainer = other.GetComponent<PlayerController>().GunWeaponContainer;
                playerScript = other.GetComponent<PlayerController>();
                //  other.gameObject.GetComponent<Bear>().coinNumber++;
                // coinCounter.instanceCoin.IncreaseCoin(+1);

                // Destroy(this.gameObject);
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(playerScript!= null)
            {
                playerScript.HasCombatWeapon = false;
                playerScript.HasBoomrang = false;
                playerScript.HasGunWeapon = false;
            }
           
            playerScript = null ;
            OutlineScript.OutlineWidth = 0;
            CombatWeaponContainer = null;
            //  other.gameObject.GetComponent<Bear>().coinNumber++;
            // coinCounter.instanceCoin.IncreaseCoin(+1);

            // Destroy(this.gameObject);
        }
    }


    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
    { 

            if (other.GetComponent<PlayerController>().HasCombatWeapon == true || other.GetComponent<PlayerController>().HasCombatWeapon == true || other.GetComponent<PlayerController>().HasGunWeapon == true)


                return;

             


            if (IsCombat)
            {

                other.GetComponent<PlayerController>().HasCombatWeapon = true;
            other.GetComponent<PlayerController>().HasBoomrang = false;
            other.GetComponent<PlayerController>().HasGunWeapon = false;


            OutlineScript.OutlineWidth = 10;
            CombatWeaponContainer = other.GetComponent<PlayerController>().CombatWeaponContainer;

            

            }

        if (IsBoomrang)
        {

            other.GetComponent<PlayerController>().HasCombatWeapon = false;
            other.GetComponent<PlayerController>().HasBoomrang = true;
            other.GetComponent<PlayerController>().HasGunWeapon = false;


            OutlineScript.OutlineWidth = 10;
            CombatWeaponContainer = other.GetComponent<PlayerController>().CombatWeaponContainer;



        }


        if ( IsGun)
        {

            other.GetComponent<PlayerController>().HasCombatWeapon = false;
            other.GetComponent<PlayerController>().HasBoomrang = false;
            other.GetComponent<PlayerController>().HasGunWeapon = true;

            OutlineScript.OutlineWidth = 10;
            CombatWeaponContainer = other.GetComponent<PlayerController>().GunWeaponContainer;



        }





     }





    }
    void OnCollisionEnter(Collision collision)
    {
        if (IsDrop && collision.gameObject.CompareTag("floor"))
           
        {
            YPosition = transform.localPosition.y+1f;
            Invoke("restBehaviour", .5f);
        }
    }

    void restBehaviour()
    {
        IsDrop = false;
        IsPicked = false;
        timer = 0;
        OutlineScript.OutlineWidth = 0;
        CombatWeaponContainer = null;
        transform.rotation = startQuaternion;
        transform.localPosition = new Vector3(transform.localPosition.x, YPosition, transform.localPosition.z);

        //transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, movementStep);


    }

}
