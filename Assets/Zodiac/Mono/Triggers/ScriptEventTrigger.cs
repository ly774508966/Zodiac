using UnityEngine;
using System.Collections;

namespace Zodiac.Mono
{
	public class ScriptEventTrigger : TriggerMono
	{

		public bool editModeUpdate = true;

		public bool start;
		public bool update;
		public bool lateUpdate;

		protected override void OnValidate ()
		{
			runInEditMode = editModeUpdate;
			base.OnValidate ();
		}

		//called on script attached or reset
		void Reset ()
		{

		}

		void Awake ()
		{

		}

		void OnEnable ()
		{

		}

		void Start ()
		{
			if (start)
				Fire ();
		}

		protected override void Update ()
		{
			if (update)
				Fire ();
			base.Update ();
		}

		void LateUpdate ()
		{
			if (lateUpdate)
				Fire ();
		}

	}
}