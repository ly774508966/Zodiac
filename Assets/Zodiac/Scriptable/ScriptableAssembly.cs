using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

using Zodiac.Instantiator;

namespace Zodiac.Scriptable
{
	public class ScriptableAssembly : ScriptableHeader
	{

		//static int maxTypes = 100;

		public void Init (Assembly assembly, string typeFilter, InstantiatorBase _instantiator)
		{
			base.Init (Utilities.FullNameToString (assembly.FullName));
			children = GetTypes (assembly, typeFilter, _instantiator);
		}

		public static ScriptableHeader GetAssemblies (string _assemblyFilter, string _typeFilter, InstantiatorBase _instantiator)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies ().
				Where (a => a.FullName.Contains (_assemblyFilter)).
				OrderBy (a => a.FullName).
				ToList ();
			var children = new List<ScriptableBase> ();
			assemblies.ForEach (a => children.Add (CreateAssembly (a, _typeFilter, _instantiator)));
			return ScriptableHeader.CreateHeader ("Assemblies", children);
		}

		static ScriptableAssembly CreateAssembly (Assembly assembly, string typeFilter, InstantiatorBase _instantiator)
		{
			var scriptableAssembly = ScriptableObject.CreateInstance<ScriptableAssembly> ();
			scriptableAssembly.Init (assembly, typeFilter, _instantiator);
			return scriptableAssembly;
		}

		List<ScriptableBase> GetTypes (Assembly assembly, string typeFilter, InstantiatorBase _instantiator)
		{
			var types = assembly.GetTypes ().
				Where (t => t.FullName.Contains (typeFilter)).
				OrderBy (t => t.FullName).
				ToList ();
			var children = new List<ScriptableBase> ();
			types.ForEach (t => children.Add (ScriptableTypePointer.CreateTypePointer (t, t.Name, _instantiator)));
			return children;
		}



	}
}