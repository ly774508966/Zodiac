using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

using Zodiac.Mono;

namespace Zodiac.Instantiator
{

	public class PrimitiveInstantiator : InstantiatorBase
	{



		public PrimitiveInstantiator () : base ()
		{
		}

		//standalone properties
		public override void InstantiateListener (object sender, PrimitiveEventArgs e)
		{
			var instance = CreateMonoObject (e.componentType, Zodiac.GetZodiacRoot ());
			if (e.componentType.IsSubclassOf (typeof(MemberMono))) {
				((MemberMono)instance).StandaloneInit (CreateUnityObjectPointer (instance));
			} else {
				//standalone Init
				instance.Init ();
			}
		}


	}

}