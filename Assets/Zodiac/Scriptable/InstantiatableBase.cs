using UnityEngine;
using System.Collections;

using System;
using Zodiac.Instantiator;


namespace Zodiac.Scriptable
{

	public abstract class InstantiatableBase : ScriptableHeader
	{
		event InstantiateHandler instantiate;

		public bool isInstantiatable{ get { return(instantiatableType == null) ? false : true; } }

		protected Type instantiatableType;

		//Children of this class must call this init
		protected void InstantiatableInit (Type _instantiatableType, string _name)
		{
			//this will be null if no suitable type is fond
			instantiatableType = _instantiatableType;
			Init (_name);

		}

		public virtual void InstantiateFire ()
		{
			InstantiateFire (new PrimitiveEventArgs (instantiatableType));
		}

		protected void InstantiateFire (PrimitiveEventArgs args)
		{
			instantiate (this, args);
		}

		public void AddDelegate (InstantiatorBase _instantiator)
		{
			instantiate += new InstantiateHandler (_instantiator.InstantiateListener);
		}

	}
}