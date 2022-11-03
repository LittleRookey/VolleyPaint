using System;
using UnityEngine;

namespace Blobcreate.Universal
{
	public abstract class ProjectileBehaviour : MonoBehaviour
	{
		[SerializeField] protected Transform explosionFX;

		protected bool exploded = false;

		public Action<Collision> OnExplode;

		public Transform Target { get; set; }
		public Vector3 TargetPoint { get; set; }
		public virtual Transform ExplosionFX { get => explosionFX; set => explosionFX = value; }

		public bool faceDirection;

		protected Rigidbody rb;

		[SerializeField] protected bool dontDestroyOnCollision;

        private void Awake()
        {
			rb = GetComponent<Rigidbody>();
        }
        protected abstract void OnLaunch();

		protected virtual void Fly()
		{
			if (exploded)
				return;

			if (Target != null)
				TargetPoint = Target.position;

			// Do the movement here...
		}

		protected virtual void Explosion(Collision collision)
		{
			OnExplode?.Invoke(collision);
			

			if (explosionFX != null)
            {
				ContactPoint contact = collision.GetContact(0);
				GameObject hitImpactObj = Instantiate(explosionFX.gameObject, contact.point, Quaternion.FromToRotation(Vector3.up, contact.normal));
				//hitImpactObj.transform.position = contact.point;
				//hitImpactObj.transform.forward = contact.normal;
			}
		}

		public void Launch(Transform target)
		{
			exploded = false;
			Target = target;
			TargetPoint = target.position;
			OnLaunch();
		}

		public void Launch(Vector3 targetPoint)
		{
			exploded = false;
			Target = null;
			TargetPoint = targetPoint;
			OnLaunch();
		}


		void Update()
		{
			Fly();
			if (faceDirection)
            {
				transform.forward = rb.velocity;
            }
		}

		protected virtual void OnCollisionEnter(Collision collision)
		{
			if (exploded)
				return;

			exploded = true;
			Explosion(collision);
			// Unsubscribe all events and destroy self.
			OnExplode = null;
			if (!dontDestroyOnCollision)
				Destroy(gameObject);
		}
	}
}