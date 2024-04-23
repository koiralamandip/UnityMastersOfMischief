using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FIMSpace;
public class mover : MonoBehaviour
{
    
    public float speed;
    public float RotSpeed=0.015F;
    float moveAmount;
  public  float animeSpeedMultipler;
    public Animator anime;
    public Rigidbody rbody;
    [SerializeField] private float accelartion = 7;
    [SerializeField] private float deccelartion = 7;
    [SerializeField] [Range(0f, 1f)] public float velocityPower = 0.9f;


    public LeaningAnimator ManualInformLeaning;
    void Start()
    {
        
    }
   
         void FixedUpdate()
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Vector3 movement = new Vector3(-Input.GetAxisRaw("Horizontal"), 0.0f, -Input.GetAxisRaw("Vertical"));
        //transform.Translate(movement * speed * Time.deltaTime, Space.World);


        float inputMovement = -Input.GetAxis("Horizontal");
        float targetspeed = inputMovement * speed;
        float speedDif = targetspeed - rbody.velocity.x;
        float accelrate = (Mathf.Abs(targetspeed) > 0.01f) ? accelartion : deccelartion;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelrate, velocityPower) * Mathf.Sign(speedDif);
        rbody.AddForce(movement * Vector2.right);



        float inputMovement2 = -Input.GetAxis("Vertical");
        float targetspeed2 = inputMovement2 * speed;
        float speedDif2 = targetspeed2 - rbody.velocity.z;
        float accelrate2 = (Mathf.Abs(targetspeed2) > 0.01f) ? accelartion : deccelartion;

        float movement2 = Mathf.Pow(Mathf.Abs(speedDif2) * accelrate2, velocityPower) * Mathf.Sign(speedDif2);
        rbody.AddForce(movement2 * Vector3.forward);





        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
      




    }
    // Update is called once per frame
    void Update()
    {
        //Vector3 inputt = new Vector3(-Input.GetAxisRaw("Vertical"),0 , Input.GetAxisRaw("Horizontal"));
        //transform.position = transform.position + inputt.normalized * speed * Time.deltaTime;
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputt).normalized, Time.deltaTime * RotSpeed);


        if (Input.GetAxis("Horizontal") !=0 || Input.GetAxis("Vertical") !=0) { 

            moveAmount += Time.deltaTime * animeSpeedMultipler;

        }
        else
        {
            moveAmount -= Time.deltaTime * animeSpeedMultipler *5;
        }
        moveAmount = Mathf.Clamp(moveAmount, 0, 1);

        anime.SetFloat("speed", moveAmount);


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Vector3 movement = new Vector3(-Input.GetAxisRaw("Horizontal"), 0.0f, -Input.GetAxisRaw("Vertical"));

        //transform.Translate(movement * speed * Time.deltaTime, Space.World);




        if (ManualInformLeaning)
        {
            if (rbody.velocity.magnitude > 0.1f)
                ManualInformLeaning.SetIsAccelerating = true;
            else
                ManualInformLeaning.SetIsAccelerating = false;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////



       
}
     void LateUpdate()
    {
        Vector3 ROTI = new Vector3(-Input.GetAxisRaw("Vertical"), 0.0f, Input.GetAxisRaw("Horizontal"));


        if (ROTI != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(ROTI), RotSpeed);
        }
    }
}
