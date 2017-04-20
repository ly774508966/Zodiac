using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Object = UnityEngine.Object;

using System.Linq;

using Zodiac.Instantiator;

namespace Zodiac.Scriptable
{

	public class ScriptableGameObjectPointer : ScriptableObjectPointer
	{

		public static ScriptableGameObjectPointer CreateGameObjectPointer (GameObject _pointer, InstantiatorBase _instantiator)
		{
			var goPointer = ScriptableObject.CreateInstance<ScriptableGameObjectPointer> ();
			goPointer.Init (_pointer, _instantiator);
			return goPointer;
		}

		public void Init (GameObject _pointer, InstantiatorBase _instantiator)
		{
			//maybe error prone approach
			Init ((Object)_pointer, _pointer.name, _instantiator);
			children.Add (GetComponents (_pointer, _instantiator));
			children.Add (GetChildren (_pointer, _instantiator));
		}

		ScriptableHeader GetComponents (GameObject go, InstantiatorBase _instantiator)
		{
			var scriptableComponents = new List<ScriptableBase> ();
			foreach (var component in go.GetComponents<Component>()) {
				//mesh is not instantiated until play so these cause trouble
				if (component is MeshFilter || component is MeshRenderer)
					continue;
				var scriptableComponent = ScriptableObject.CreateInstance<ScriptableObjectPointer> ();
				var sName = Utilities.ObjectTypeToString (component);
				scriptableComponent.Init (component, sName, _instantiator);
				scriptableComponents.Add (scriptableComponent);
			}
			return ScriptableHeader.CreateHeader ("Components", scriptableComponents);
		}

		ScriptableHeader GetChildren (GameObject go, InstantiatorBase _instantiator)
		{
			var scriptableChildren = new List<ScriptableBase> ();
			foreach (var child in go.transform.Cast<Object>()) {
				var scriptableChild = ScriptableObject.CreateInstance<ScriptableObjectPointer> ();
				scriptableChild.Init (child, child.name, _instantiator);
				scriptableChildren.Add (scriptableChild);
			}
			return ScriptableHeader.CreateHeader ("Children", scriptableChildren);
		}
			
	}
}