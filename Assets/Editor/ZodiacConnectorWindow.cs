using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using UnityEditor;

using Zodiac.Mono;
using Zodiac;

namespace Zodiac.ImmediateModeGUI
{

	public class ZodiacConnectorWindow : ZodiacWindowBase
	{

		ZodiacConnector zodiacConnector;

		string selectedContext;
		string fireContext;

		[MenuItem ("Window/Zodiac Connector")]
		public static void ShowWindow ()
		{
			EditorWindow.GetWindow (typeof(ZodiacConnectorWindow), false, "Connector", true);
		}


		protected override void OnEnable ()
		{
			zodiacConnector = Zodiac.GetZodiacRoot ().gameObject.GetComponent<ZodiacConnector> ();
			base.OnEnable ();

		}

		protected override void OnGUI ()
		{
			base.OnGUI ();
			if (zodiacConnector == null)
				zodiacConnector = Zodiac.GetZodiacRoot ().gameObject.GetComponent<ZodiacConnector> ();
			zodiacConnector.enabled = EditorGUILayout.Toggle ("Enabled", zodiacConnector.enabled);
			if (!zodiacConnector.enabled || !zodiacConnector)
				return;
			drawContext ();
			drawSelected ();
		}

		void setContext ()
		{
			selectedContext = zodiacConnector.selectedContextLabel;
			fireContext = zodiacConnector.fireContextLabel;
		}

		void drawSelected ()
		{
			GUILayout.Label ("Selected Objects", EditorStyles.boldLabel);
			GUILayout.BeginHorizontal (GUILayout.Width (10));
			GUILayout.BeginVertical ();
			zodiacConnector.m_selected.ForEach (s => GUILayout.Label (s.name));
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();
		}

		void drawContext ()
		{
			GUILayout.Label (selectedContext, EditorStyles.boldLabel);
			GUILayout.BeginHorizontal ();
			GUILayout.Label (fireContext, EditorStyles.boldLabel, GUILayout.Width (100));
			if (fireContext != "") {
				if (GUILayout.Button ("Fire (debug)", GUILayout.Width (100)))
					zodiacConnector.Fire ();
			}
			GUILayout.EndHorizontal ();
		}

	}
}