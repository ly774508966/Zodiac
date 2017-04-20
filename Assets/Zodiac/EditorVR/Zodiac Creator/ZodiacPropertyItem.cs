using UnityEngine;
using System.Collections;

using UnityEditor;
using UnityEditor.Experimental.EditorVR.Workspaces;
using UnityEditor.Experimental.EditorVR.Data;

using Zodiac.Mono;

namespace Zodiac.EditorVR
{
	class ZodiacPropertyItem : InspectorPropertyItem
	{

		protected MemberMono memberMono;

		public override void Setup (InspectorData data)
		{
			base.Setup (data);
			memberMono = (MemberMono)data.serializedObject.targetObject;
		}
			
	}



}