using System;
using Blobcreate.Universal;
using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class SimpleExplosive : ProjectileBehaviour
	{
		[SerializeField] public float radius = 4f;
		[SerializeField] public float centerForce = 10f;
		[SerializeField] public float forceUplit;
		[SerializeField] public LayerMask scanMask;
		[SerializeField] public int damage = 100;
		[SerializeField] public bool isPlayerWeapon;
		[SerializeField] public LayerMask selfMask;
		[SerializeField] public float selfDamageRatio = 0.25f;

		[SerializeField] Collider[] result = new Collider[16];

		[SerializeField] private float maxFallingVelocity;

		[SerializeField] public Transform netCollider;
		[SerializeField] public bool followBall;
		private void Update()
        {

			if (  followBall)
			{

				netCollider.position = this.transform.position;

			}

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

}