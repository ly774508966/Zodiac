#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using System.Reflection;

using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Data;

using Object = UnityEngine.Object;

using Zodiac.Scriptable;
using Zodiac.Mono;

namespace Zodiac.EditorVR
{

	[MainMenuItem ("Zodiac Creator", "Create", "Add functionality to your scene")]
	class ZodiacCreatorWorkspace : ZodiacCreatorWorkspaceBase
	{

		ZodiacCreator zodiacCreator;

		public override void Setup ()
		{
			base.Setup ();
			zodiacCreator = Zodiac.GetZodiacRoot ().GetComponent<ZodiacCreator> ();
			OnSelectionChanged ();
		}


		protected override List<InspectorData> GetNewData ()
		{
			var objs = zodiacCreator.GetData ();
			return	ScriptableListToInspectorDataList (objs);
		}


		List<InspectorData> ScriptableListToInspectorDataList (List<ScriptableBase> sobjs)
		{
			var iobjs = new List<InspectorData> ();
			//not sure how these are getting in but thisll do for now
			sobjs.RemoveAll (sobj => sobj == null);
			sobjs.ForEach (sobj => iobjs.Add (ObjectToInspectorData (sobj)));
			return iobjs;
		}


		InspectorData ObjectToInspectorData (Object obj)
		{
			var children = new List<InspectorData> ();
			var sobj = new SerializedObject (obj);
			var property = sobj.GetIterator ();
			while (property.NextVisible (true)) {
				if (validSerializedProperty (property))
					children.Add (SerializedPropertyToPropertyData (property, sobj));
				//in the case of a Header, this will add a list of scriptables
			}
			if (obj is ScriptableHeader) {
				var scriptableChildren = ((ScriptableHeader)obj).children;
				children.AddRange (ScriptableListToInspectorDataList (scriptableChildren));
				if (obj is InstantiatableBase)
					return new InspectorData ("InstantiatableItem", sobj, children);
				return new InspectorData ("ZodiacItem", sobj, children);
			}
			return new InspectorData ("InspectorComponentItem", sobj, children);
		}

		bool validSerializedProperty (SerializedProperty property)
		{
			if (property.depth != 0)
				return false;
			switch (property.displayName) {
			case "Children":
			case "Script":
			case "Foldout":
			case "Scroll Pos":
				return false;
			}
			return true;
		}


		PropertyData SerializedPropertyToPropertyData (SerializedProperty property, SerializedObject obj)
		{
			//Debug.Log (obj.targetObject.GetType ());
			string template;
			List<InspectorData> children = null;
			//Debug.Log (property.propertyType.ToString ());
			switch (property.propertyType) {
			case SerializedPropertyType.Vector2:
			case SerializedPropertyType.Vector3:
			case SerializedPropertyType.Vector4:
			case SerializedPropertyType.Quaternion:
				template = (obj.targetObject is MemberMono) ? "ZodiacVectorItem" : "InspectorVectorItem";
				break;
			case SerializedPropertyType.Integer:
				goto case SerializedPropertyType.Float;
			case SerializedPropertyType.Float:
				template = (obj.targetObject is MemberMono) ? "ZodiacNumberItem" : "InspectorNumberItem";
				break;
			case SerializedPropertyType.Character:
			case SerializedPropertyType.String:
				template = "InspectorStringItem";
				break;
			case SerializedPropertyType.Bounds:
				template = "InspectorBoundsItem";
				break;
			case SerializedPropertyType.Boolean:
				template = "InspectorBoolItem";
				break;

			case SerializedPropertyType.Color:
				template = "InspectorColorItem";
				break;
			case SerializedPropertyType.Rect:
				template = "InspectorRectItem";
				break;
			case SerializedPropertyType.LayerMask:
			case SerializedPropertyType.Enum:
				template = "InspectorDropDownItem";
				break;
			case SerializedPropertyType.ObjectReference:

			//this is not the cause of the type mismatch
				//possibly here pointers could be created also
				template = (property.objectReferenceValue is ScriptableBase) ?
				"ZodiacItem"
				: "InspectorObjectFieldItem";
				children = GetSubProperties (property, obj);
				break;
			case SerializedPropertyType.Generic:
			//this may be applicable to all children
				children = GetSubProperties (property, obj);
				template = property.isArray
				? "InspectorArrayHeaderItem"
				: "InspectorGenericItem";
			//i cant find inspectorgenericitem
				break;

			default:
				template = "InspectorUnimplementedItem";
				break;
			}

			//children = GetSubProperties (property, obj);
			var propertyData = new PropertyData (template, obj, children, property.Copy ());

			if (children != null)
				AddChildrenChangingCallback (propertyData);
			return propertyData;
		}

		//think of a list as an object and each item is a property
		List<InspectorData> GetSubProperties (SerializedProperty property, SerializedObject obj)
		{
			var children = new List<InspectorData> ();
			var iteratorProperty = property.Copy ();
			while (iteratorProperty.NextVisible (true)) {
				if (iteratorProperty.depth == 0)
					break;

				switch (iteratorProperty.propertyType) {
				case SerializedPropertyType.ArraySize:
				//in this case dont want folks messin with array size
				//children.Add (new PropertyData ("InspectorNumberItem", obj, null, iteratorProperty.Copy ()));
					break;
				default:
					children.Add (SerializedPropertyToPropertyData (iteratorProperty, obj));
					break;
				}
			}

			return children;
		}


		/*this functionality will most probably be moved to Zodiac Creator
		protected InspectorData GameObjectToInspectorData (GameObject go)
		{
			var children = new List<InspectorData> ();
			foreach (var component in go.GetComponents<Component>()) {
				children.Add (ObjectToInspectorData (component));
			}
			return new InspectorData ("InspectorHeaderItem", new SerializedObject (go), children);
		}
		*/

	}
}
#endif