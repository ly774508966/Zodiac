using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zodiac.Mono
{
	public class UnityObjectPointer : PointerMono
	{

		[SerializeField]
		UnityEngine.Object uobjPointer;

		[SerializeField]
		LinkRenderer uobjLinker;

		public void Init (UnityEngine.Object _pointer)
		{
			uobjPointer = _pointer;
			//Deserialize ();	methinks unnessecary
			base.Init ();
			dataType = uobjPointer.GetType ().AssemblyQualifiedName;
			name = uobjPointer.name + " " + Utilities.ObjectTypeToString (uobjPointer) + " pointer";

			uobjLinker = LinkRenderer.AddLinkRenderer (this, LinkerType.Pointer);
		}


		protected override void Update ()
		{
			if (uobjPointer is Component) {
				uobjLinker.Show (((Component)uobjPointer).transform);
			} else if (uobjPointer is GameObject) {
				uobjLinker.Show (((GameObject)uobjPointer).transform);
			}

			base.Update ();
		}

		public override System.Type GetPointerType ()
		{
			return uobjPointer.GetType ();
		}


		public override object GetBoxedData ()
		{
			return 	(object)uobjPointer;
		}


		public override void SetBoxedData (object obj)
		{
			uobjPointer = ((UnityEngine.Object)obj);
		}

	}
}