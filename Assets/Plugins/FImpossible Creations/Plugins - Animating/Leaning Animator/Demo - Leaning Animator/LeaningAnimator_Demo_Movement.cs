using FIMSpace.Basics;
using UnityEngine;
using UnityEngine.SceneManagement;
using RootMotion.FinalIK;
using FIMSpace.BonesStimulation;
using RootMotion;
using Blobcreate.ProjectileToolkit.Demo;
using RootMotion.Dynamics;


using System.Collections;
using System.Collections.Generic;
 
using UnityEditor;

namespace FIMSpace
{
    public class LeaningAnimator_Demo_Movement : MonoBehaviour
    {
        [Header("Abbility")]
        public bool canRagdoll;
        public bool canPunch;

        [Header("Movment")]
        [SerializeField] private Transform oriantation;
        Vector3 directions;
     
      
        
        [SerializeField] private BonesStimulator spineBoneStimulator;
        [SerializeField] private float RotationSpeed = 8f;
        [SerializeField] private float groundDrag;
        private float inputMovement;
        [SerializeField] private float speed = 8f;
        [SerializeField] private float sprinting;
        private bool RotateInDir = true;
        private float offsetRotY = 0f;
        private Vector3 moveDir = Vector3.zero;
        private Vector3 targetRot;
        protected float rotateSpeed = 10f;
        protected Quaternion smoothedRotation;
        public bool isGrounded = false;
        Rigidbody rigbody;

        public bool standingOnPlatfom;
        Rigidbody moveableRIGI;
       

        [Header("Slop")]
        [SerializeField] private float RayRange;
        [SerializeField] private float angle;
        [SerializeField] private float maxSlopeAngle;
        [SerializeField] private bool OnSlope;
        RaycastHit slopeHit;
        [SerializeField] private float slopeSpeed;

    
     
       


      
        /// /////////////////////////

        [Header("Layar Mask")]

        [SerializeField] private Transform feetPos;
        [SerializeField] private float checkRadius;
        [SerializeField] private LayerMask whatIsGround;

        [Header("Jump")]
        [SerializeField] private float jumpForce;
        bool jumpRequest;
        [SerializeField] private float greavity;
        [SerializeField] private float jumpFallMultiper;
        float valu;
        [SerializeField] private float jumpBufferTime = 0.2f;
         private float jumpBufferCounter;
         [SerializeField] private bool isJumping;
        /////////////////////////////////
        [Header("Animator")]
        float moveAmount;
        [SerializeField] private float animeSpeedMultipler;
         private Animator Anime;
       
        [SerializeField] private LookAtIK LookIK;
       
       



        [Header("Pick Up Objects")]
        [SerializeField] private Rigidbody BoxPrefab;
        [SerializeField] private Rigidbody BallPrefab;
        [SerializeField] private bool FoundPickup ;
        [SerializeField] private Transform pickableObject;
        [SerializeField] private Transform objectPosition;
        FullBodyBipedIK fullbodyik;
        [SerializeField] private Transform arcLuncher;
        private ProjectileLauncher arcLuncherCode;
          private CapsuleCollider capsulCollider;
        [Header("Ragdoll")]
       public bool activeRagdoll;
        [SerializeField] private PuppetMaster puppetMaster;
        [SerializeField] private Transform EntireCharacterCntroller;
        [SerializeField] private Transform hips;

        [Header("Combat And Weapons")]
        [SerializeField] private BonesStimulator rightHandBone;

        void Start()
        {
            capsulCollider= this.GetComponent<CapsuleCollider>();

            Anime = this.GetComponent<Animator>();

            arcLuncherCode = arcLuncher.GetComponent<ProjectileLauncher>();
            arcLuncher.gameObject.SetActive(false);
            fullbodyik = this.GetComponent<FullBodyBipedIK>();
            fullbodyik.solver.IKPositionWeight = 0;
           
            targetRot = transform.rotation.eulerAngles;

            
            

            ////////////////////////////////////
            ///
            rigbody = GetComponent<Rigidbody>();
           

            // Making character slide on colliders when jumping forward vertical walls etc.
            
            rigbody.interpolation = RigidbodyInterpolation.Interpolate; // Interpolation for smooth motion with rig.velocity and rig.angularVelocity changes
            rigbody.maxAngularVelocity = 100f; // Allowing angular velocity rotate fast towards target rotation

           // smoothedAcceleration = Vector3.zero;
            smoothedRotation = transform.rotation;

            ////////////////////////////////
            spineBoneStimulator.enabled = true;
        }
        private void LateUpdate()
        {
            
        }
        private void FixedUpdate()
        {



           if (standingOnPlatfom)
            {
                rigbody.MovePosition(rigbody.position + moveableRIGI.velocity * Time.deltaTime);
              

               
                if (rigbody.velocity.z > 0 && Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
                {
                    
                    rigbody.velocity = Vector3.zero;
                }

            }
            
            /////////////////////////////////////// satnding on moving platfom 

            Ray ray1 = new Ray(transform.position, Vector3.down);

            RaycastHit hit1;
            if (Physics.Raycast(ray1, out hit1, 0.3f))
            {


                if (hit1.transform.name == "moveingplatform" && isGrounded)
                {

                    moveableRIGI = hit1.collider.gameObject.GetComponent<Rigidbody>();
                 
                    standingOnPlatfom = true;


                    /*
					 RigidPlatform rigidPlatform = hit.transform.GetComponent<RigidPlatform>();
					 RigidParent rigidParent = rigidPlatform.rigidParent;
					 player.transform.SetParent(rigidParent.transform);

					*/



                    



                }
                else
                {
                    standingOnPlatfom = false;
                }

                 

            }

           
            ////////////////////////// moving in direction of slop
            ///

            if (Onslope())
            {


               
                    rigbody.AddForce(GetSlopeMoveDirection() * speed * slopeSpeed, ForceMode.Force);

                
            }




            ///////////////////////////////////////////////////

            float inputMovement = Input.GetAxisRaw("Horizontal");
           

            float inputMovement2 = Input.GetAxisRaw("Vertical");

             directions = oriantation.forward * -inputMovement2 + oriantation.right * -inputMovement;
            rigbody.AddForce(directions.normalized * speed * 10, ForceMode.Force);

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") != 0 || Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") != 0)
            {
                
                rigbody.AddForce(directions.normalized * sprinting * 10, ForceMode.Force);
                Anime.SetFloat("speed", 2);
                RotationSpeed = 14;
            }
            else
            {
                RotationSpeed = 8;
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////
            Anime.SetBool("isgrounded", isGrounded);

            Anime.SetBool("isjumping", isJumping);
            if (isJumping)
            {
                
                valu += Time.deltaTime;
                
            }
            else
            {
                valu = 0;
                 
            }
            Anime.SetFloat("jumpVAL", valu);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //Vector3 movement = new Vector3(-Input.GetAxisRaw("Horizontal"), 0.0f, -Input.GetAxisRaw("Vertical"));

            //transform.Translate(movement * speed * Time.deltaTime, Space.World);





            //////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///

            ////////////////////////////////////////// Triggering jump to be executed in next fixed update
            /*
             rigbody.AddForce(Physics.gravity * (greavity - 1) * rigbody.mass);


             if (isGrounded && Input.GetKeyDown(KeyCode.Space))
             {
                 rigbody.velocity = new Vector2(rigbody.velocity.x, jumpForce);

             }

             if (Input.GetKeyUp(KeyCode.Space) && rigbody.velocity.y > 0)
             {


                 rigbody.velocity = new Vector2(rigbody.velocity.x, rigbody.velocity.y / jumpFallMultiper);


             }






             if (isGrounded)
            {

           
            float inputMovement = Input.GetAxis("Horizontal");
            float targetspeed = inputMovement * speed;
            float speedDif = targetspeed - rigbody.velocity.x;
            float accelrate = (Mathf.Abs(targetspeed) > 0.01f) ? accelartion : deccelartion;

            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelrate, velocityPower) * Mathf.Sign(speedDif);
            rigbody.AddForce(movement * Vector2.right);



            float inputMovement2 = Input.GetAxis("Vertical");
            float targetspeed2 = inputMovement2 * speed;
            float speedDif2 = targetspeed2 - rigbody.velocity.z;
            float accelrate2 = (Mathf.Abs(targetspeed2) > 0.01f) ? accelartion : deccelartion;

            float movement2 = Mathf.Pow(Mathf.Abs(speedDif2) * accelrate2, velocityPower) * Mathf.Sign(speedDif2);
            rigbody.AddForce(movement2 * Vector3.forward);



                

            }






            if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
            {

                speed = 7f;

            }
            else
            {
                speed = Orginalspeed;
            }





            */


            Ray ray2 = new Ray(transform.position, Vector3.down);
            RaycastHit boxHit;

            if (Physics.Raycast(ray2, out boxHit, .75f))
            {
                if (boxHit.transform.tag == "BOX")
                {
                     
                }

            }
            else
            {
                 
            }




            isGrounded = Physics.CheckSphere(feetPos.position, checkRadius, whatIsGround) ;
          

          



            if (jumpRequest)
            {
                 
                //rb.velocity += Vector2.up * jumpVelocity;
                rigbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
                jumpBufferCounter = 0f;
                jumpRequest = false;
            }
          

            rigbody.AddForce(Physics.gravity * (greavity - 1) * rigbody.mass);






            ///////////////////////////////////////////////////////////////////////
            rigbody.angularVelocity = FEngineering.QToAngularVelocity(smoothedRotation * Quaternion.Inverse(transform.rotation)) * rotateSpeed * 10f;




        }
        void Update()
        {

            ////////////////////////////////// grab the cat ragdoll
           
            Ray rayu = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitu;

            if (Physics.Raycast(rayu, out hitu, 100) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.DrawLine(rayu.origin, hitu.point, Color.white);

                if(hitu.transform.tag == "Player")
                {
                     // PickupRagdoll = true;
                }
                

            }
              


            /////////////////////////// ragdoll

            if (Input.GetKeyUp(KeyCode.C) && canRagdoll)
            {
              
                activeRagdoll = !activeRagdoll;
                
            }

             
                if (activeRagdoll)
                {
                    enableRagdoll();
                EntireCharacterCntroller.position = Vector3.MoveTowards(EntireCharacterCntroller.position, hips.position, Time.deltaTime * 23f);

                Vector3 movePosition = Vector3.Slerp(rigbody.position, hips.position, 22 * Time.fixedDeltaTime);
                rigbody.MovePosition(movePosition);
                
                puppetMaster.transform.SetParent(null);
                  }
                else
                {
                    DisableRagdoll();

                }
             
           

            /////////////////////////////////////////////////
            SpeedControl();
            // handle drag
            if (isGrounded)
            {
            rigbody.drag = groundDrag;
                 
            }
                
            else
            {
           rigbody.drag = 0;
            }

        

            ////////////////////////////////////////////// anime


            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {

                moveAmount += Time.deltaTime * animeSpeedMultipler;

            }
            else
            {
                moveAmount -= Time.deltaTime * animeSpeedMultipler ;
            }
            moveAmount = Mathf.Clamp(moveAmount, 0, 1);
            if (!Input.GetKey(KeyCode.LeftShift)){
                Anime.SetFloat("speed", moveAmount);

            }


            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                LookIK.solver.bodyWeight = 0;

            }
            else
            {
                LookIK.solver.bodyWeight = 1;
            }





            ////////////////////////////////////////////////////////// jumping
            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }
            


            if (isGrounded == true && jumpBufferCounter > 0f)
            {

                jumpRequest = true;
                isJumping = true;

            }
            if (Input.GetKeyUp(KeyCode.Space) && rigbody.velocity.y > 0)
            {


                rigbody.velocity = new Vector2(rigbody.velocity.x, rigbody.velocity.y / jumpFallMultiper);


            }





            ////////////////////////////////
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            //////////////////////////


           // moveSpeed = MovementSpeed;

            moveDir = Vector3.zero;
            offsetRotY = 0f;

            
                // Move forward / back
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                 {
                moveDir += Vector3.forward; 
             

                 }
                
                     
                else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                 {
             moveDir += Vector3.back;
               

                 }
                    

                // Move left / right
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                 {
              moveDir += Vector3.left;
               

                  }
                   
                else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                   {
                moveDir += Vector3.right;
                

                   }
                   

            // Debug sprint
            //  if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) moveSpeed *= 1.5f;

           




            // Defining rotation for object
            if (moveDir != Vector3.zero)
            {
                moveDir.Normalize();

                if (RotateInDir)
                {
                   
                    targetRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.main.transform.TransformDirection(moveDir), Vector3.up)).eulerAngles;
                    moveDir = Vector3.forward;
                }
                else
                    targetRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up)).eulerAngles;

                targetRot.y += offsetRotY;


                 

            }


            // Calculating smooth acceleration value to be used in next fixed update frame
           // smoothedAcceleration = Vector3.SmoothDamp(smoothedAcceleration, moveDir, ref veloHelper, accelerationTime, Mathf.Infinity, Time.deltaTime);

            // Calculating smooth rotation to be applied in fixes update
            smoothedRotation = Quaternion.Lerp(rigbody.rotation, Quaternion.Euler(targetRot), Time.deltaTime * RotationSpeed);










            ///////////////////////////////////// SLOP

            OnSlope = Onslope();

            if (Input.GetKeyDown(KeyCode.Space))
            {
            RayRange = 0;
            return;

            }
            else if (Onslope() && isGrounded)
            {

                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                    {
                        rigbody.velocity = Vector3.zero;
                    greavity = 0.5f;

                    }
                    else
                    {
                        rigbody.isKinematic = false;
                    greavity = 5f;

                }
            }


            if (Onslope() == false)
            {
                rigbody.isKinematic = false;
                greavity = 5f;
            }

            ////////////////////////////////////// punching

            if (Input.GetKeyDown(KeyCode.Mouse0) && !FoundPickup && Anime.GetBool("isPunching") == false && canPunch)
            {

                StartCoroutine(resetPunch());

            }


            ///////////////////////////////////pick up objects






            if (pickableObject == null)
                return;
          else

            if (Input.GetKeyDown(KeyCode.E)  && pickableObject.transform != null)
            {
                FoundPickup = !FoundPickup;
                 
            }
            if (FoundPickup == false  )
            {
                capsulCollider.center = new Vector3(0.01624298f, 1.00417f, -0.0005250571f);
                capsulCollider.radius = 0.5770756f;
                pickableObject.GetComponent<Collider>().enabled = true;
                
                Invoke("dropBox", 0.1f);

                if (fullbodyik.solver.IKPositionWeight >= 0)
                
                    fullbodyik.solver.IKPositionWeight -= Time.deltaTime * 4;




               



             }


            if (FoundPickup && Input.GetKeyUp(KeyCode.Mouse0)  )
            {

                Destroy(pickableObject.gameObject);
                fullbodyik.solver.IKPositionWeight  = 0;
                Invoke("shootBox", 0.4f);

            }


                if (FoundPickup)
            {
                capsulCollider.center = new Vector3(0.01624f , 1.00417f, 0.3158f);
                capsulCollider.radius = 0.8934f;
                pickableObject.GetComponent<Collider>().enabled = false;
                transform.GetComponent<CapsuleCollider>().enabled = false;
                pickableObject.transform.rotation = Quaternion.identity;

                if (fullbodyik.solver.IKPositionWeight >= 1)
                {
                    pickableObject.position = objectPosition.position;
                }
                if (fullbodyik.solver.IKPositionWeight <= 1)
                    fullbodyik.solver.IKPositionWeight += Time.deltaTime*7;
               
                 if (fullbodyik.solver.IKPositionWeight >= 1)
                {
                    arcLuncher.gameObject.SetActive(true);
                }
                  
            }

             

            /*

            Input.GetKeyUp(KeyCode.E)
                   Invoke("shootBox", 0.2f);
             Invoke("dropBox", 0.2f);
                if (FoundPickup  )
               {

                 transform.GetComponent<CapsuleCollider>().isTrigger = false; // triger box
                pickableObject.position = objectPosition.position;
                pickableObject.transform.rotation = Quaternion.identity;
                pickableObject.GetComponent<Collider>().enabled = false;
                pickableObject.GetComponent<Rigidbody>().isKinematic = true;
                fullbodyik.solver.IKPositionWeight = 1;
                arcLuncher.gameObject.SetActive(true);
                
              

            }

            */





        }







        private void SpeedControl()
        {
            Vector3 flatVel = new Vector3(rigbody.velocity.x, 0f, rigbody.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > speed)
            {
                Vector3 limitedVel = flatVel.normalized * speed;
                rigbody.velocity = new Vector3(limitedVel.x, rigbody.velocity.y, limitedVel.z);
            }
        }


        void OnCollisionEnter(Collision collisionInfo)
        {
            //if (collisionInfo.collider.tag == "floor")
            
                isJumping = false;
                 RayRange = 0.2f;


        }
        void OnTriggerStay (Collider other)
        {
            
            if (other.gameObject.tag == "BOX")
            {
                pickableObject = other.gameObject.transform;

                arcLuncherCode.bulletPrefab = BoxPrefab;
            }


            if (other.gameObject.tag == "ball")
            {
                pickableObject = other.gameObject.transform;

                arcLuncherCode.bulletPrefab = BallPrefab;
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "BOX")
            {
                pickableObject = null;

                arcLuncherCode.bulletPrefab = null;
            }


            if (other.gameObject.tag == "ball")
            {
                pickableObject = null;

                arcLuncherCode.bulletPrefab = null;
            }
        }


        void dropBox()
        {

            transform.GetComponent<CapsuleCollider>().enabled = true;
          
             arcLuncher.gameObject.SetActive(false);


         
                


        }
        void shootBox()
        {


           

            transform.GetComponent<CapsuleCollider>().enabled = true;
             arcLuncher.gameObject.SetActive(false);
            FoundPickup = false;
            pickableObject = null;

            capsulCollider.center = new Vector3(0.01624298f, 1.00417f, -0.0005250571f);
            capsulCollider.radius = 0.5770756f;
             
                fullbodyik.solver.IKPositionWeight = 0;
             

        }
        private bool Onslope()
        {

            Ray rayu = new Ray(transform.position, Vector3.down);
       

            if (Physics.Raycast(rayu, out slopeHit, RayRange))
            {
                

                if (slopeHit.transform.name == "slop"){ 
                Debug.DrawLine(rayu.origin, slopeHit.point, Color.blue);

                 angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
                }

            }

             
            return false;
        }


        private Vector3 GetSlopeMoveDirection()
        {
            return Vector3.ProjectOnPlane(directions, slopeHit.normal).normalized;
        }

        void enableRagdoll()
        {

             

            
            



            puppetMaster.mode = PuppetMaster.Mode.Active;
            puppetMaster.state = PuppetMaster.State.Dead;

            /*
            anime.enabled = false;

            foreach (var rigidodyies in ragdollRigidbodies)
            {
                rigidodyies.isKinematic = false;
            }



            foreach (var coliders in ragdollColliders)
            {
                coliders.enabled = true;
            }


            foreach (var BonesStimulatorSS in boneCodes)
            {
                BonesStimulatorSS.enabled = false;
            }
            */





        }



        private void DisableRagdoll()
        {
            puppetMaster.mode = PuppetMaster.Mode.Disabled;
            puppetMaster.state = PuppetMaster.State.Alive;
            puppetMaster.transform.parent = EntireCharacterCntroller.transform;
            rigbody.isKinematic = false;
            /*
            anime.enabled = true;
            foreach (var rigidodyies in ragdollRigidbodies)
            {
                rigidodyies.isKinematic = true;
            }



            foreach (var coliders in ragdollColliders)
            {
                coliders.enabled = false;
            }


            foreach (var BonesStimulatorSS in boneCodes)
            {
                BonesStimulatorSS.enabled = true;
            }

            */
        }


        IEnumerator resetPunch()
        {

            Anime.SetLayerWeight(1, 1);
            Anime.SetBool("isPunching", true);
            rightHandBone.enabled = false;

            yield return new WaitForSeconds(.6f);

            Anime.SetLayerWeight(1, 0);
            Anime.SetBool("isPunching", false);
            rightHandBone.enabled = true;
        }



    }
}