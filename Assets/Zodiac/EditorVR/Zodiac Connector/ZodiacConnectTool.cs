#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine;
using UnityEngine.InputNew;

using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Tools;
using Zodiac.Mono;

using System.Linq;


namespace Zodiac.EditorVR
{

	[MainMenuItem ("Zodiac Connector", "Create", "Connect Zodiac Objects")]
	class ZodiacConnectTool : MonoBehaviour, ITool, IConnectInterfaces, IInstantiateMenuUI,
	IUsesRayOrigin, ISelectionChanged
	{
		
		[SerializeField]
		ZodiacConnectMenu m_MenuPrefab;

		GameObject m_ToolMenu;


		public ZodiacConnector m_zodiacConnector;

		public Func<Transform,IMenu,GameObject> instantiateMenuUI{ private get; set; }

		ZodiacConnectMenu m_connectMenu;

		public Transform rayOrigin { get; set; }

		void Start ()
		{
			m_ToolMenu = instantiateMenuUI (rayOrigin, m_MenuPrefab);
			m_connectMenu = m_ToolMenu.GetComponent<ZodiacConnectMenu> ();
			this.ConnectInterfaces (m_connectMenu, rayOrigin);
			m_connectMenu.fire = m_zodiacConnector.Fire;
			m_zodiacConnector = Zodiac.GetZodiacRoot ().GetComponent<ZodiacConnector> ();
		}

		public void OnSelectionChanged ()
		{
			m_connectMenu.UpdateSelectedText (m_zodiacConnector.m_selected);
		}

		void OnDestroy ()
		{
			Debug.Log ("DESTRUCTION!");
			ObjectUtils.Destroy (m_ToolMenu);
		}
	}
}
#endif
