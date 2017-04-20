using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

using Zodiac.Scriptable;

namespace Zodiac
{

	public delegate void InstantiateHandler (object sender, PrimitiveEventArgs e);

	public class PrimitiveEventArgs: EventArgs
	{
		public Type componentType{ get; set; }

		public PrimitiveEventArgs (Type _componentType)
		{
			componentType = _componentType;
		}
	}

	public class MemberEventArgs:PrimitiveEventArgs
	{

		public ScriptableMemberBase scriptableMember;

		public MemberEventArgs (ScriptableMemberBase _scriptableMember, Type _componentType) : base (_componentType)
		{
			scriptableMember = _scriptableMember;
		}
	}


}