using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Linq;
using System.Reflection;

using Zodiac.Instantiator;
using Zodiac.Mono;
using Object = UnityEngine.Object;

using MethodList = System.Collections.Generic.List<System.Reflection.MethodInfo>;

namespace Zodiac.Scriptable
{

	public class ScriptableMethod : ScriptableMemberBase
	{
		
		public ScriptableHeader returnType;
		public ScriptableHeader parameterOptions;

		public void Init (Object _obj, MethodList _methodInfos)
		{
			MemberInit (_obj, _methodInfos.First (), typeof(MethodMono));
			returnType = GetReturnType (_methodInfos.First ());
			parameterOptions = GetParameterOptions (_methodInfos);
			children.Add (returnType);
			children.Add (parameterOptions);
		}

		public ScriptableHeader GetReturnType (MethodInfo method)
		{
			var children = new List<ScriptableBase> ();
			children.Add (ScriptableHeader.CreateEmptyHeader (method.ReturnType.AssemblyQualifiedName));
			var name = "Return Type: " + Utilities.TypeToString (method.ReturnType);
			return ScriptableHeader.CreateHeader (name, children);

		}

		public ScriptableHeader GetParameterOptions (MethodList methods)
		{
			var children = new List<ScriptableBase> ();
			foreach (var method in methods) {
				var parametersString = Utilities.ParameterArrayToString (method.GetParameters ());
				children.Add (ScriptableHeader.CreateEmptyHeader (parametersString));
			}
			return ScriptableHeader.CreateHeader ("Parameter Options", children);
		}


		public static ScriptableHeader GetObjectMethods (Object _obj, InstantiatorBase _instantiator)
		{
			return CreateScriptableMethods (_obj, _obj.GetType (), _instantiator);
		}

		public static ScriptableHeader GetTypeMethods (Type type, InstantiatorBase _instantiator)
		{
			return CreateScriptableMethods (null, type, _instantiator);
		}


		static ScriptableHeader CreateScriptableMethods (Object _obj, Type type, InstantiatorBase _instantiator)
		{
			var methodDictionary = GetMethodDictionary (type);
			var scriptableMethods = new List<ScriptableBase> ();
			foreach (var kvp in methodDictionary) {
				var scriptableMethod = ScriptableObject.CreateInstance<ScriptableMethod> ();
				scriptableMethod.Init (_obj, kvp.Value);
				scriptableMethod.AddDelegate (_instantiator);
				scriptableMethods.Add (scriptableMethod);
			}

			return ScriptableHeader.CreateHeader ("Methods", scriptableMethods);
		}

		static Dictionary<string,MethodList> GetMethodDictionary (Type type)
		{
			var methodDictionary = new Dictionary<string,MethodList> ();
			foreach (var method in type.GetMethods().OrderBy(m =>m.Name)) {
				if (!validMethod (method))
					continue;

				MethodList methodList;
				if (methodDictionary.TryGetValue (method.Name, out methodList)) {
					methodList.Add (method);
				} else {
					methodList = new MethodList ();
					methodList.Add (method);
					methodDictionary.Add (method.Name, methodList);
				}
			}
			return methodDictionary;
		}

		static bool validMethod (MethodInfo method)
		{
			if (method.Name.Contains ("get_")
			    || method.Name.Contains ("set_")
			    || method.IsGenericMethod)
				return false;
			return true;
		}
	}
}
