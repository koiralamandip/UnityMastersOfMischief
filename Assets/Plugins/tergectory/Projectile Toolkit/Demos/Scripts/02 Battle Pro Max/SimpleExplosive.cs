using System;
using Blobcreate.Universal;
using UnityEngine;

namespace Blobcreate.ProjectileToolkit.Demo
{
	public class SimpleExplosive : ProjectileBehaviour
	{
		public float radius = 4f;
		public float centerForce = 10f;
		public float forceUplit;
		public LayerMask scanMask;
		public int damage = 100;
		public bool isPlayerWeapon;
		public LayerMask selfMask;
		public float selfDamageRatio = 0.25f;

		Collider[] result = new Collider[16];

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