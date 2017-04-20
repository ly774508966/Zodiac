using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace Zodiac.Mono
{
	public abstract class PointerMono : ValueMono
	{

	

		protected override void OnValidate ()
		{
			base.OnValidate ();
			GetComponentsInChildren<MemberMono> ().ToList ().ForEach (p => p.Deserialize ());
		}




		public virtual Type GetPointerType ()
		{
			return null;
		}



	}
}