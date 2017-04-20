#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor.Experimental.EditorVR;

using UnityEditor;
using UnityEngine.UI;
using Zodiac.Mono;

//using System.Linq;

namespace Zodiac.EditorVR
{
	class ZodiacConnectMenu : MonoBehaviour, IMenu
	{
		
		public Action fire;

		[SerializeField]
		Text connectContext;

		[SerializeField]
		Text fireContext;

		[SerializeField]
		List<Text> selectedText;

		public bool visible {
			get { return gameObject.activeSelf; }
			set { gameObject.SetActive (value); }
		}

		public GameObject menuContent {
			get { return gameObject; }
		}

		public void Fire ()
		{
			fire ();
		}

		public void UpdateSelectedText (List<ZodiacMono> selected)
		{
			for (int i = 0; i < selectedText.Count; i++) {
				if (selected.Count - 1 <= i) {
					selectedText [i].text = "";
					continue;
				}
				selectedText [i].text = (selected [i] == null) ? "" : selected [i].name;
			}
		}

		public void UpdateContext (ZodiacConnector connector)
		{
			connectContext.text = connector.selectedContextLabel;
			fireContext.text = connector.fireContextLabel;
		}

	}
}
#endif
