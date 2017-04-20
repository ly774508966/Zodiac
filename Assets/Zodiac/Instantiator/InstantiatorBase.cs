using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEditor;

using Zodiac.Mono;

using Object = UnityEngine.Object;

namespace Zodiac
{

	public abstract class InstantiatorBase : MonoBehaviour
	{
		//public bool goawaywarnings;

		public virtual void InstantiateListener (object sender, PrimitiveEventArgs e)
		{

		}

		protected ZodiacMono CreateMonoObject (Type _type, Transform parent)
		{
			if (!_type.IsSubclassOf (typeof(ZodiacMono)))
				return null;
			
			var monoPrefab = Resources.Load ("ZodiacMono");
			var go = (monoPrefab == null) ? new GameObject () : (GameObject)GameObject.Instantiate (monoPrefab);
			go.name = "";
			go.transform.parent = parent;
			var zodiacMono = (ZodiacMono)go.AddComponent (_type);
			return zodiacMono;
		}



		protected UnityObjectPointer CreateUnityObjectPointer (UnityEngine.Object uobj)
		{
			var pointers = Zodiac.GetZodiacRoot ().GetComponentsInChildren<UnityObjectPointer> ();
			foreach (var pointer in pointers) {
				if (pointer.GetBoxedData () == uobj)
					return pointer;
			}
			var newPointer = (UnityObjectPointer)CreateMonoObject (typeof(UnityObjectPointer), Zodiac.GetZodiacRoot ());
			newPointer.Init (uobj);
			return newPointer;
		}

		protected PointerMono CreateStaticPointer (Type type)
		{
			var pointer = (StaticPointer)CreateMonoObject (typeof(StaticPointer), Zodiac.GetZodiacRoot ());
			pointer.Init (type);
			return pointer;
		}

		protected void AddValueGizmo (string name, Transform parent)
		{
			Object prefab = null;

			if (name.Contains ("Position") || name.Contains ("position")) {
				prefab = Resources.Load ("value_position");
			} else if (name.Contains ("Rotation") || name.Contains ("rotation")) {
				prefab = Resources.Load ("value_rotation");
			} else if (name.Contains ("Scale") || name.Contains ("scale")) {
				prefab = Resources.Load ("value_scale");
			} else if (name.Contains ("Color") || name.Contains ("color")) {
				prefab = Resources.Load ("value_color");
			} else if (name.Contains ("Vector3") || name.Contains ("vector3")) {
				
			}
			if (prefab != null)
				GameObject.Instantiate (prefab, parent);
		}

	}

}