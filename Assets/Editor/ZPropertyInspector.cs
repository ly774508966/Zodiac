using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

using System;
using System.Reflection;

using Zodiac.Mono;

using System.Linq;

[CustomEditor (typeof(MemberMono), true)]
[CanEditMultipleObjects]
public class ZPropertyInspector : Editor
{

	MemberMono member;
	PropertyInfo data;


	void OnEnable ()
	{
		member = (MemberMono)target;
		//data = member.GetType ().GetProperty ("data");
	}


	public override void OnInspectorGUI ()
	{
		if (member.pointer is UnityObjectPointer)
			GUILayout.Label ("Pointer Name: " + ((UnityEngine.Object)member.pointer.GetBoxedData ()).name);
		GUILayout.Label ("Data Type: " + member.dataType);
		GUILayout.Label ("Pointer Type: " + member.pointer.GetPointerType ().ToString ());
		GUILayout.Label ("Get: " + member.getCallbackName);
		GUILayout.Label ("Set: " + member.setCallbackName);


		if (member is Vector3Mono) {
			var vector3Mono = (Vector3Mono)member;

			//disable setting of data when it is arithmetic product
			if ((vector3Mono.arithmeticFactors != null && vector3Mono.arithmeticFactors.Any ()) || (vector3Mono.FloatFactors != null && vector3Mono.FloatFactors.Any ())) {
				EditorGUILayout.Vector3Field ("Arithmetic Result", vector3Mono.data);
			} else {
				vector3Mono.data = EditorGUILayout.Vector3Field ("Value", vector3Mono.data);
			}

		} else if (member is FloatMono) {
			var floatMono = (FloatMono)member;
			floatMono.data = EditorGUILayout.FloatField ("Value", floatMono.data);

		}


		//allowed for debugging only
		base.OnInspectorGUI ();
	}


}
