using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEditor;

using Zodiac.Scriptable;

using Object = UnityEngine.Object;

namespace Zodiac.ImmediateModeGUI
{

	public class ZodiacCreatorWindow : ZodiacWindowBase
	{
		
		ZodiacCreator zodiacCreator;

		Vector2 scrollPos;

		List<ScriptableBase> data;

		[MenuItem ("Window/Zodiac Creator")]
		public static void ShowWindow ()
		{
			EditorWindow.GetWindow (typeof(ZodiacCreatorWindow), false, "Creator", true);
		}


		protected override void OnEnable ()
		{
			zodiacCreator = Zodiac.GetZodiacRoot ().gameObject.GetComponent<ZodiacCreator> ();
			base.OnEnable ();
		}

		protected override void OnGUI ()
		{
			base.OnGUI ();
			if (data == null)
				OnSelectionChanged ();

			//GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height * 2));
			//scrollPos = GUILayout.BeginScrollView (scrollPos, GUILayout.MinHeight (Screen.height * 2));

			DrawScriptableObjects (data);

			//GUILayout.EndScrollView ();
			//GUILayout.EndArea ();
		}

		protected override void OnSelectionChanged ()
		{
			data = zodiacCreator.GetData ();
		}

		void DrawScriptableObjects (List<ScriptableBase> _objects)
		{
			foreach (var obj in _objects) {
				if (obj == null)
					continue;
				if (obj is ScriptableHeader) {
					DrawScriptableHeader ((ScriptableHeader)obj);
				} else {
					DrawScriptableObject (obj);
				}
			}
		}

		void DrawScriptableObject (ScriptableBase obj)
		{
			GUILayout.BeginHorizontal ();
			GUILayout.Label (obj.name, GUILayout.Width (200));
			GUILayout.EndHorizontal ();
		}

		void DrawInstantiateButton (InstantiatableBase obj)
		{
			if (GUILayout.Button ("Instantiate", GUILayout.Width (100)))
				obj.InstantiateFire ();
		}

		void DrawScriptableHeader (ScriptableHeader header)
		{
			EditorGUILayout.Separator ();
			GUILayout.Box ("", GUILayout.ExpandWidth (true), GUILayout.Height (1));
			GUILayout.BeginHorizontal ();
			if (header.children.Count == 0) {
				GUILayout.Label (header.name);
			} else {
				header.foldout = EditorGUILayout.Foldout (header.foldout, header.name, EditorStyles.foldout);
			}
			if (header is InstantiatableBase)
				DrawInstantiateButton ((InstantiatableBase)header);
			GUILayout.EndHorizontal ();
			if (header.foldout) {
				GUILayout.BeginHorizontal ();
				GUILayout.Space (10);
				GUILayout.BeginVertical ();
				//GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height * 2));
				header.scrollPos = GUILayout.BeginScrollView (header.scrollPos, GUILayout.ExpandHeight (true), GUILayout.MinHeight (200), GUILayout.MaxHeight (Screen.height));//, GUILayout.MinHeight (100));
				DrawScriptableObjects (header.children);
				GUILayout.EndScrollView ();
				GUILayout.EndVertical ();
				GUILayout.EndHorizontal ();
			}
				
		}

	}
}