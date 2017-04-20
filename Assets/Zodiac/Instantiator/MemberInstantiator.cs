using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;

//using System.Reflection;
using System.Linq;

using Zodiac.Mono;
using Zodiac.Scriptable;

using Object = UnityEngine.Object;

namespace Zodiac.Instantiator
{

	public class MemberInstantiator : PrimitiveInstantiator
	{
		
		public override void InstantiateListener (object sender, PrimitiveEventArgs e)
		{
			if (e is MemberEventArgs) {
				var args = (MemberEventArgs)e;
				var zPointer = (args.scriptableMember.parentObject == null) ? 
					CreateStaticPointer (args.scriptableMember.parentType) 
					: CreateUnityObjectPointer (args.scriptableMember.parentObject);
				if (args.scriptableMember is ScriptableProperty) {
					CreateProperty (args, zPointer);
					return;
				} else if (args.scriptableMember is ScriptableMethod) {
					CreateMethod (args, zPointer);
					return;
				}
			}
			base.InstantiateListener (sender, e);
		}

		void CreateProperty (MemberEventArgs e, PointerMono _pointer)
		{
			var property = (ScriptableProperty)e.scriptableMember;
			var propertyMono = (MemberMono)CreateMonoObject (e.componentType, _pointer.transform);
			AddValueGizmo (property.name, propertyMono.transform);
			propertyMono.Init (property.propertyInfo, _pointer);
		}

		void CreateMethod (MemberEventArgs e, PointerMono _pointer)
		{
			var method = (ScriptableMethod)e.scriptableMember;
			var methodMono = (MethodMono)CreateMonoObject (typeof(MethodMono), _pointer.transform);
			AddValueGizmo (method.name, methodMono.transform);
			methodMono.Init (method, _pointer);
		}


	}
}