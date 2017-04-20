using UnityEngine;
using System.Collections;

using System;
using System.Reflection;

using Object = UnityEngine.Object;

using Zodiac.Instantiator;

namespace Zodiac.Scriptable
{

	public abstract class ScriptableMemberBase : InstantiatableBase
	{
		public Object parentObject;
		public Type parentType;
		//public MemberInfo memberInfo;

		//Children of this class must call this init
		public void MemberInit (Object _obj, MemberInfo memberInfo, Type _zType)
		{
			InstantiatableInit (_zType, memberInfo.Name);
			parentObject = _obj;
			parentType = memberInfo.ReflectedType;
		}

		public override void InstantiateFire ()
		{
			InstantiateFire (new MemberEventArgs (this, instantiatableType));
		}


		public virtual Type GetMemberType ()
		{
			return null;
		}

	}
}