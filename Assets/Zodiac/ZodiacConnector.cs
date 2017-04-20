using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zodiac.Mono;

using UnityEditor;


using System;

namespace Zodiac
{

	[ExecuteInEditMode]
	public class ZodiacConnector : MonoBehaviour
	{
		//lists can ForEach
		public List<ZodiacMono> m_selected;

		public bool containsTrigger{ get { return m_selected.Any (s => s is TriggerMono); } }

		public string fireContextLabel{ get { return (containsTrigger) ? "Fire At Will" : "Select an Event"; } }

		public string selectedContextLabel{ get { return m_selected.Count + " Object" + ((m_selected.Count == 1) ? "" : "s") + " selected"; } }

		public void OnEnable ()
		{
			m_selected = new List<ZodiacMono> ();

		}

		public void OnDrawGizmos ()
		{
			if (enabled)
				Update ();
		}

		public void Update ()
		{
			if (!Selection.activeObject)
				m_selected.Clear ();
			m_selected.AddRange (
				Selection.gameObjects.SelectMany (g => g.GetComponents<ZodiacMono> ()).
				Where (z => !m_selected.Contains (z)).ToList ());
			if (m_selected.Count > 1)
				Connect ();
		}

		void Connect ()
		{
			m_selected.First ().Connect (m_selected.Skip (1));
			m_selected.RemoveAll (s => s != m_selected.First ());
			Selection.activeGameObject = m_selected.First ().gameObject;
		}

		public void Fire ()
		{
			var events = m_selected.Where (c => c is TriggerMono).Cast<TriggerMono> ().ToList ();
			events.ForEach (s => s.Fire ());
		}





	}
}