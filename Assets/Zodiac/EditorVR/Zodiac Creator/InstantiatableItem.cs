using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using UnityEditor.Experimental.EditorVR.Data;
using Zodiac.Scriptable;

using Button = UnityEditor.Experimental.EditorVR.UI.Button;

namespace Zodiac.EditorVR
{

	class InstantiatableItem : ZodiacItem
	{

		[SerializeField]
		Button m_Instantiate;

		public void Instantiate ()
		{
			if (!(data.serializedObject.targetObject is InstantiatableBase)) {
				Debug.LogError ("Assigned object is not instantiatable", transform);
				return;
			}
			var target = (InstantiatableBase)data.serializedObject.targetObject;
			target.InstantiateFire ();
		}

	}
}