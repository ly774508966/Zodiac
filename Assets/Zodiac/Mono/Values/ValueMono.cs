using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Zodiac.Mono
{

	public abstract class ValueMono : ZodiacMono
	{

		//QUALIFIED ASSEMBLY NAME
		//
		public string dataType;

		public override void Init ()
		{
			base.Init ();
		}

		public virtual object GetBoxedData ()
		{
			Debug.LogWarning ("null get boxed data, did you forget to override?", this);
			return new object ();
		}


		public virtual void SetBoxedData (object obj)
		{
			Debug.LogWarning ("null set boxed data, did you forget to override?", this);
		}

	}
}