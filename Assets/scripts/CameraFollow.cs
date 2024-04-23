using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("camera follow")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform terejctoryTarget;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    [SerializeField] private Transform objectForLookIK;
    [SerializeField] private float offsetLookik = 8.4f;

    [Header("camera look")]
    [SerializeField] private float minimumX = -20f;
    [SerializeField] private float maximumX = 20f;
    [SerializeField] private float minimumY = -35f;
    [SerializeField] private float maximumY = 35f;
    [SerializeField] private float damping = 2.0f;
    [SerializeField] private float mouseSen;
    float XROTION;
    float YROTION;
    [Header("camera pan and zoom")]

    [SerializeField] private bool isZooming;
    [SerializeField] private float scrollSpeed = 2;
    [SerializeField] private float minZoom = 8;
    [SerializeField] private float maxZoom = -14;
    [SerializeField] private float frontAccelertion ;
    [SerializeField] private float BackAccelertion;
    [SerializeField] private bool isPaning;
    [SerializeField] private float PanSpeed;


     
    void FixedUpdate()
    {
       if(isZooming == false && isPaning==false)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

        }

       








    }
    private void LateUpdate()
    {

      
        
         
    }
    void Start()
    {
        
    }

     
    void Update()
    {


        ///////////////////////// zoom and pan

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) // forward
        {
            isZooming = false;
            isPaning = false;
            frontAccelertion = 0;
            BackAccelertion = 0;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f) // forward
        {
            isZooming = true;

        }


        if (Input.GetMouseButton(1))
        {
           
            isPaning = true;
        }


       


        if (isZooming)
        {
            /*
            Vector3 pos = transform.position;
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                pos = pos - transform.forward;
                transform.position = pos;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                pos = pos + transform.forward;
                transform.position = pos;


              //if (transform.localPosition.z <= minZoom)
            }
            */
            frontAccelertion = Mathf.Clamp(frontAccelertion, 0, 4);
            BackAccelertion = Mathf.Clamp(BackAccelertion, 0, 4);
            if (Vector3.Distance(transform.position, terejctoryTarget.transform.position) >= minZoom)
            {

                if (frontAccelertion > 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, terejctoryTarget.position, Time.deltaTime * scrollSpeed);
                }
            }

            
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                frontAccelertion += Time.deltaTime*17;

                  

                }
                 

               if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {

                    BackAccelertion += Time.deltaTime * 17;
                    
                }



            if (Input.GetAxis("Mouse ScrollWheel") < 0 && frontAccelertion >0)
            {
                frontAccelertion =0;



            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && BackAccelertion > 0)
            {
                BackAccelertion = 0;



            }




            if (transform.localPosition.z >= maxZoom)
            { 

                if (BackAccelertion > 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, terejctoryTarget.position, Time.deltaTime * -scrollSpeed);

                }
            }

            if (BackAccelertion > 0 && Input.GetAxis("Mouse ScrollWheel") == 0)
            {
                BackAccelertion -= Time.deltaTime;
            }
            if (frontAccelertion > 0 && Input.GetAxis("Mouse ScrollWheel") == 0)
            {
                frontAccelertion -= Time.deltaTime;
            }

        }

        if (isPaning)
        {

            // transform.Translate(Vector3.right * -Input.GetAxis("Mouse X") * PanSpeed);
            //transform.Translate(transform.up * -Input.GetAxis("Mouse Y") * PanSpeed, Space.World);

             
            if (Input.GetMouseButton(1))
            {
                float speed = PanSpeed * Time.deltaTime;
                Camera.main.transform.position -= new Vector3(Input.GetAxis("Mouse X") * speed, Input.GetAxis("Mouse Y") * speed,0 );

                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * damping);

            }
            


            

        }

        ////////////////////////////////////////// camera follow
        Vector3 scrennpos = Input.mousePosition;
        scrennpos.z = Camera.main.nearClipPlane + offsetLookik;
        Vector3 pos = Camera.main.ScreenToWorldPoint(scrennpos);
        objectForLookIK.position = pos;


        //////////////////////////////// camera look x and y

        if (!Input.GetMouseButton(1)) { 
            float mosX = Input.GetAxis("Mouse X") * mouseSen * Time.deltaTime;
            float mosY = Input.GetAxis("Mouse Y") * mouseSen * Time.deltaTime;

            XROTION -= mosY;
            YROTION -= mosX;
            XROTION = Mathf.Clamp(XROTION, minimumX, maximumX);//// limittion for x 
            YROTION = Mathf.Clamp(YROTION, minimumY, maximumY);


            var desiredRotQ = Quaternion.Euler(XROTION, -YROTION, 0);

            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotQ, Time.deltaTime * damping);

        }



        ///////////////////////////////////////// arch trejectory

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            terejctoryTarget.position = hit.point;

            
        }

        ////////////////////////////////////////

    }




    




}
