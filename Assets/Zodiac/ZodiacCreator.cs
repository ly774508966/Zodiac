using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using Zodiac.Instantiator;
using Zodiac.Scriptable;
using Zodiac.Mono;

namespace Zodiac
{

	public class ZodiacCreator :MemberInstantiator
	{

		public string AssemblyFilter = "UnityEngine";
		public string typeFilter = "Vector";

		public List<ScriptableBase> GetData ()
		{
			var data = new List<ScriptableBase> ();
			if (Selection.activeGameObject) {
				var scriptableGo = ScriptableGameObjectPointer.CreateGameObjectPointer (Selection.activeGameObject, this);
				data.Add (scriptableGo);
			} else {
				data.Add (ScriptablePrimitive.GetPrimitives (this));
				data.Add (ScriptableAssembly.GetAssemblies (AssemblyFilter, typeFilter, this));
			}
			RemoveNullObjects (data);
			return data;
		}

		//may no longer be nessecary
		public void RemoveNullObjects (List<ScriptableBase> objs)
		{
			objs.RemoveAll (o => o == null);
			foreach (var obj in objs) {
				if (obj is ScriptableHeader)
					RemoveNullObjects (((ScriptableHeader)obj).children);
			}
		}


		//To be implemented in scriptableObjectstuff

		//This will be implemented when EditorVR has a keyboard
		/*for now the functionality can be accessed through an editor window
		ObjList GetAssemblyObjectTypes ()
		{
			return new ObjList ();
		}

		*/
	}
}