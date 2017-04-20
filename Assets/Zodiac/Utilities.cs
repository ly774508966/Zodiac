using UnityEngine;
using System.Collections;

using System;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

public static class Utilities
{

	public static string FullNameToString (string fullName)
	{
		return fullName.Split (',').First ();
	}

	public static string ObjectTypeToString (object obj)
	{
		return obj.GetType ().ToString ().Split ('.').Last ();
	}

	public static string TypeToString (Type type)
	{
		return type.ToString ().Split ('.').Last ();
	}

	public static string ParameterArrayToString (ParameterInfo[] parameters)
	{
		var val = "";
		if (parameters.Length > 0) {
			val = ParameterToString (parameters [0]);
			for (int i = 1; i < parameters.Length; i++) {
				val += ", " + ParameterToString (parameters [i]);
			}
		} 
		return val;
	}

	static string ParameterToString (ParameterInfo parameter)
	{
		string val = TypeToString (parameter.ParameterType);
		val += " " + parameter.Name;
		return val;
	}

	public static string TypeArrayToString (Type[] types)
	{
		string val = TypeToString (types [0]);
		if (types.Length == 1)
			return val;
		for (int i = 1; i < types.Length; i++) {
			val += ", " + TypeToString (types [i]);
		}
		return val;
	}
	/*
	public static Type[] StringToStringArray (string val)
	{
		string[] names = val.Split (", ", StringSplitOptions.RemoveEmptyEntries);
	}
	*/

}

