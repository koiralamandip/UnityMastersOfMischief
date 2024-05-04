using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blobcreate.ProjectileToolkit.Demo;
using Blobcreate.Universal;
public class TrajectoryObject : ProjectileBehaviour
{
	 

	[SerializeField] private float maxFallingVelocity;

	
	private void Update()
	{

		

	}
	protected override void OnLaunch()
	{
	}

	// Apply damage and force.
	protected override void Explosion(Collision collision)
	{

	}
	private void FixedUpdate()
	{
		Vector3 temp = this.GetComponent<Rigidbody>().velocity;
		if (this.GetComponent<Rigidbody>().velocity.y < maxFallingVelocity)
		{

			temp.y = maxFallingVelocity;
			this.GetComponent<Rigidbody>().velocity = temp;


		}

	}
}

