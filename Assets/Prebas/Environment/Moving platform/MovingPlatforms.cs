using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
	public List<Transform> points;
	public Transform platform;
	public int goalPoint = 0;
	
	public float currentSpeed = 0;
	public float accel = 0.1f;
	public float timeToWait;
	float orgispeed;





	void LateUpdate()
	{


 
	}






	void Start()

	{
		 
		
		orgispeed = currentSpeed;
	}
	void Update()
	{



		 


		
		 //platform.position = Vector3.Lerp(platform.transform.position, points[goalPoint].position, Time.deltaTime * currentSpeed);

		}
	void FixedUpdate()
	{

   platform.position = Vector2.MoveTowards(platform.position, points[goalPoint].position, Time.deltaTime * currentSpeed);


		 
		 //	Vector3 position = Vector3.MoveTowards(platform.gameObject.GetComponent<Rigidbody>().position, points[goalPoint].position, Time.fixedDeltaTime * currentSpeed);
		// platform.gameObject.GetComponent<Rigidbody>().MovePosition(position);


		 





		//currentSpeed = Mathf.Clamp(currentSpeed, 0, 4f);





		//currentSpeed += accel * Time.deltaTime;    // limit to 1 for "full speed"













		/*
		if (currentSpeed == 0){

             this.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
         {
       this.GetComponent<Rigidbody>().isKinematic = true;
        }
		 
		*/


		if (Vector2.Distance(platform.position, points[goalPoint].position) < 0.1f)
		{
			

			goalPoint++;
			 currentSpeed = 0;

			Invoke("gotonext", timeToWait);
		 
			 

			if (goalPoint >= points.Count) {
				goalPoint = 0;

				currentSpeed = 0;

				Invoke("gotonext", timeToWait);

			}





        }
       



	}
	void gotonext(){
		 
		currentSpeed = orgispeed;

	}
	 
 

}