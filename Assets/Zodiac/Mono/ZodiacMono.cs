using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Random = UnityEngine.Random;

namespace Zodiac.Mono
{
	[ExecuteInEditMode]
	public abstract class ZodiacMono : MonoBehaviour, ISerializationCallbackReceiver
	{


		public virtual void Init ()
		{
			name = Utilities.ObjectTypeToString (this).Replace ("Mono", "");
			float radius = transform.parent.TransformPoint (Vector3.one).magnitude;
			Vector2 point = RandomPointOnCircle (radius);
			transform.position = transform.parent.TransformPoint (new Vector3 (point.x, 0, point.y));
			transform.rotation = Random.rotation;
		}

		void OnEnable ()
		{
			if (name == "")
				return;
			Deserialize ();
		}

		public virtual void Connect (IEnumerable<ZodiacMono> _selected)
		{
			
		}

		protected virtual void OnValidate ()
		{
			Deserialize ();
		}

		//to ensure updates in editor
		void OnDrawGizmos ()
		{
			Update ();
		}

		protected virtual void Update ()
		{
			transform.RotateAround (transform.parent.position, transform.parent.up, transform.localScale.magnitude * 0.1f);
		}

		public virtual void Deserialize ()
		{

		}

		public virtual void Serialize ()
		{


		}


		public void OnBeforeSerialize ()
		{
			Serialize ();
		}

		public void OnAfterDeserialize ()
		{
			//currently using OnEnable and OnValidate as it is called on the main thread
			//allows more flexibility, but is possibly unnessecary
			//Deserialize();
		}

		Vector2 RandomPointOnCircle (float radius)
		{
			var theta = Random.Range (0, Mathf.PI * 2);
			var x = Mathf.Cos (theta) * radius;
			var y = Mathf.Sin (theta) * radius;
			return new Vector2 (x, y);
		}

	}
}