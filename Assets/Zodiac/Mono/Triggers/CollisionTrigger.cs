using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zodiac.Mono
{
	public class PhysicsTrigger : TriggerMono
	{
		public bool fixedUpdate;
		public bool collisionEnter;
		public bool collisionStay;
		public bool collisionExit;

		void FixedUpdate ()
		{
			if (fixedUpdate)
				Fire ();
		}

		void OnCollisionEnter ()
		{
			if (collisionEnter)
				Fire ();
		}

		//void OnCollisionEnter (Collision collision)

		void OnCollisionStay ()
		{
			if (collisionStay)
				Fire ();
		}

		//void OnCollisionStay (Collision collision)

		void OnCollisionExit ()
		{
			if (collisionExit)
				Fire ();
		}

		//void OnCollisionExit (Collision collision)


	}
}