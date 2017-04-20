using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Linq;
using System.Reflection;

using Zodiac.Instantiator;
using Zodiac.Mono;

namespace Zodiac.Scriptable
{

	public class ScriptablePrimitive : InstantiatableBase
	{

		//public string valueType;

		public void Init (Type _zType)
		{
			InstantiatableInit (_zType, _zType.Name);
			//var dataProperty = _zType.GetProperty ("data");
			//valueType = (dataProperty == null) ? "null" : dataProperty.PropertyType.ToString ();
		}


		public static ScriptableHeader GetPrimitives (PrimitiveInstantiator _instantiator)
		{
			var primitives = new List<ScriptableBase> ();
			foreach (var zType in GetZTypes()) {
				var primitive = ScriptableObject.CreateInstance<ScriptablePrimitive> ();
				primitive.Init (zType);
				primitive.AddDelegate (_instantiator);
				primitives.Add (primitive);
			}
			return ScriptableHeader.CreateHeader ("Zodiac Types", primitives);
		}

		public static List<Type> GetZTypes ()
		{
			return typeof(ZodiacMono).Assembly.GetTypes ().
				Where (t => (t.IsSubclassOf (typeof(ZodiacMono))
			&& !t.IsAbstract))
				.ToList ();
		}

	}

}