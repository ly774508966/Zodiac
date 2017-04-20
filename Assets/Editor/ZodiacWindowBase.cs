using UnityEngine;
using System.Collections;

using UnityEditor;

namespace Zodiac.ImmediateModeGUI
{
	public class ZodiacWindowBase : EditorWindow
	{
		Object m_selected;

		protected virtual void OnEnable ()
		{
			m_selected = Selection.activeObject;
			OnSelectionChanged ();
		}

		protected virtual void OnGUI ()
		{
			if (m_selected != Selection.activeObject) {
				m_selected = Selection.activeObject;
				OnSelectionChanged ();
			}
		}

		protected virtual void  OnSelectionChanged ()
		{
		}

	}
}