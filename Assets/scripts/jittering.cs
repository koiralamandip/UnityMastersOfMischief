using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jittering : MonoBehaviour
{

    [Header("Recoil")]
    Vector3 intialRot;
    public Vector3 kickMinMax  ;
    
    public float recoilMoveSettleTime = 0.2f;
   
    Vector3 recoilSmoothDampVelocity;
     float recoilRotSmoothDampVelocity;
     
    Vector3 vic = Vector3.zero;
    int chanceOfRecoil;

    public Transform targtePosition;
    public float speed;
    [SerializeField] private float FireRate;
    private float NextTimeToFire = 0f;
    void Start()
    {
        intialRot = transform.localEulerAngles;

    }

    void FixedUpdate()
    {

    }
    void Update()
    {

      

            


            if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") !=0 )   && Time.time >= NextTimeToFire)
        {
                NextTimeToFire = Time.time + 1f / FireRate;
                Shoot();

        }

        transform.position = Vector3.SmoothDamp(transform.position, targtePosition.position, ref recoilSmoothDampVelocity, recoilMoveSettleTime);


    }



    void LateUpdate()
    {

       

       
         
       // transform.position = Vector3.MoveTowards(transform.position, targtePosition.position, speed * Time.deltaTime);
    }




    public void Shoot()
    {


        chanceOfRecoil = Random.Range(1, 7);


        
        if (chanceOfRecoil == 1)
        {
            vic = Vector3.up;
        }
        if (chanceOfRecoil == 2)
        {
            vic = Vector3.forward;
        }
        if (chanceOfRecoil == 3)
        {
            vic = Vector3.left;
        }
        if (chanceOfRecoil == 4)
        {
            vic = Vector3.down;
        }
        if (chanceOfRecoil == 5)
        {
            vic = Vector3.right;
        }
        if (chanceOfRecoil == 6)
        {
            vic = -Vector3.forward;
        }

      //  transform.localPosition -= vic * Random.Range(kickMinMax.x, kickMinMax.y);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, vic * Random.Range(kickMinMax.x, kickMinMax.y ), ref recoilSmoothDampVelocity, recoilMoveSettleTime);

    }

}



