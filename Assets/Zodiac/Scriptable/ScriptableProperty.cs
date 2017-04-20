using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Linq;
using System.Reflection;

using Object = UnityEngine.Object;

using Zodiac.Instantiator;

namespace Zodiac.Scriptable
{

	public class ScriptableProperty : ScriptableMemberBase
	{

		public PropertyInfo propertyInfo;

		public ScriptableHeader propertyType;
		public ScriptableHeader value;

		public void Init (Object _obj, PropertyInfo _property)
		{
			propertyInfo = _property;
			MemberInit (_obj, _property, getZTypeMatch (_property));
			propertyType = ScriptableHeader.CreateEmptyHeader ("Property Type: " + _property.PropertyType.ToString ());
			string valueString = (GetValue () == null) ? "null" : GetValue ().ToString ();
			value = ScriptableHeader.CreateEmptyHeader (valueString);
			children.Add (propertyType);
			children.Add (value);
		}

		Type getZTypeMatch (PropertyInfo _property)
		{
			foreach (var zType in ScriptablePrimitive.GetZTypes().Where(t =>t.GetProperty("data") != null)) {
				if (zType.GetProperty ("data").PropertyType == _property.PropertyType)
					return zType;
			}
			return null;
		}

		public override Type GetMemberType ()
		{
			return propertyInfo.PropertyType;
		}

		public object GetValue ()
		{
			object obj = null;
			try {
				obj = propertyInfo.GetValue (parentObject, null);
			} catch {
				//all kinds of stuff, im not worried
			}
			return obj;
		}

		public static ScriptableHeader GetTypeProperties (Type type, InstantiatorBase _instantiatorr)
		{
			return CreateScriptableProperty (null, type, _instantiatorr);
		}

		public static ScriptableHeader GetObjectProperties (Object obj, InstantiatorBase _instantiator)
		{
			return CreateScriptableProperty (obj, obj.GetType (), _instantiator);
		}

		//obj will be null for static types
		static ScriptableHeader CreateScriptableProperty (Object obj, Type type, InstantiatorBase _instantiator)
		{
			var scriptableProperties = new List<ScriptableBase> ();
			foreach (var property in type.GetProperties().OrderBy (p => p.Name)) {
				if (!validProperty (property))
					continue;
				var scriptableProperty = ScriptableObject.CreateInstance<ScriptableProperty> ();
				scriptableProperty.Init (obj, property);
				if (scriptableProperty.isInstantiatable) {
					scriptableProperties.Add (scriptableProperty);
					scriptableProperty.AddDelegate (_instantiator);
				}
			}
			return ScriptableHeader.CreateHeader ("Properties", scriptableProperties);
		}

		static bool validProperty (PropertyInfo p)
		{
			//does this need the linq Count()?
			if (p.GetIndexParameters ().Count () > 0)
				return false;
			//|| !property.CanRead
			//|| !property.CanWrite)
			// || p.Name.Contains ("sleepAngularVelocity")
			// || p.Name.Contains ("sleepVelocity")
			//   || p.Name.Contains ("useConeFriction"))

			switch (p.Name) {
		#if UNITY_EDITOR
			case "mesh":
			case "material":
			case "materials":
				return false;
		#endif
			case "sleepAngularVelocity":
			case "sleepVelocity":
			case "useConeFriction":
				return false;
			}

			return true;
		}

	}
}