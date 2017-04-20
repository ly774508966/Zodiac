using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace Zodiac.Mono
{
	public class StaticPointer : PointerMono
	{

		Type typePointer;

		public void Init (Type _type)
		{
			dataType = _type.AssemblyQualifiedName;
			Deserialize ();
			base.Init ();
			//dirty dirty hack, valuebase init is overriding datatype usage
			dataType = _type.AssemblyQualifiedName;
			name += "to " + Utilities.TypeToString (typePointer);
		}

		public override void Deserialize ()
		{
			base.Deserialize ();
			typePointer = Type.GetType (dataType);
		}

		public override Type GetPointerType ()
		{
			return typePointer;
		}

		public override object GetBoxedData ()
		{
			return (object)typePointer;
		}


		public override void SetBoxedData (object obj)
		{
			typePointer = ((Type)obj);
		}

	}
}