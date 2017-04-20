using UnityEngine;
using System.Collections;

using Zodiac.Instantiator;

using UnityEditor;
using System;

namespace Zodiac.Scriptable
{
	public class ScriptableTypePointer : ScriptableHeader
	{


		public void Init (Type _type, string _name, InstantiatorBase _instantiator)
		{
			base.Init (_name);
			children.Add (ScriptableProperty.GetTypeProperties (_type, _instantiator));
			children.Add (ScriptableMethod.GetTypeMethods (_type, _instantiator));
		}

		public static ScriptableTypePointer CreateTypePointer (Type _type, string _name, InstantiatorBase _instantiator)
		{
			var typePointer = ScriptableObject.CreateInstance<ScriptableTypePointer> ();
			typePointer.Init (_type, _name, _instantiator);
			return typePointer;
		}

	}

}