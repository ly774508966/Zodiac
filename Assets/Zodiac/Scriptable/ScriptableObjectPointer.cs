using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Object = UnityEngine.Object;

using System.Linq;

using Zodiac.Instantiator;

namespace Zodiac.Scriptable
{

	public class ScriptableObjectPointer : ScriptableHeader
	{
		public void Init (Object _pointer, string _name, InstantiatorBase _instantiator)
		{
			base.Init (_name);
			children.Add (ScriptableProperty.GetObjectProperties (_pointer, _instantiator));
			children.Add (ScriptableMethod.GetObjectMethods (_pointer, _instantiator));
		}

		public static ScriptableObjectPointer CreateObjectPointer (Object _pointer, string _name, InstantiatorBase _instantiator)
		{
			var objPointer = ScriptableObject.CreateInstance<ScriptableObjectPointer> ();
			objPointer.Init (_pointer, _name, _instantiator);
			return objPointer;
		}
	}
}