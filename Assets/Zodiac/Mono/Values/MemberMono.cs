using UnityEngine;
using System.Collections;

using System.Reflection;

namespace Zodiac.Mono
{
	public abstract class MemberMono : ValueMono
	{
		public PointerMono pointer;

		public string setCallbackName;
		public string getCallbackName;
		[SerializeField]
		LinkRenderer pointerLinker;

		public RelationalType relationalType;
		public ArithmeticType arithmeticType;


		//for standalone values only
		public void StandaloneInit (PointerMono _pointer)
		{
			Init (this.GetType ().GetProperty ("dataProperty"), _pointer);
		}

		public void Init (PropertyInfo _property, PointerMono _pointer)
		{
			if (_property.PropertyType != this.GetType ().GetProperty ("data").PropertyType)
				Debug.LogError ("type mismatch! i was expecting " + _property.PropertyType + " and got " + this.GetType ().GetProperty ("data").PropertyType.ToString ());
			getCallbackName = (_property.CanRead) ? _property.GetGetMethod ().Name : "";
			setCallbackName = (_property.CanWrite) ? _property.GetSetMethod ().Name : "";
			dataType = _property.PropertyType.AssemblyQualifiedName;
			Init (_pointer);
			name += " " + _property.Name;
		}

		public void Init (PointerMono _pointer)
		{
			pointer = _pointer;
			transform.parent = pointer.transform;
			transform.localScale *= 0.5f;
			//will call subclass ie.property init
			Init ();
			pointerLinker = LinkRenderer.AddLinkRenderer (this, LinkerType.Pointer);
			Deserialize ();
		}


		public virtual bool GetRelationalResult ()
		{
			return true;
		}


		protected override void Update ()
		{
			base.Update ();
			pointerLinker.Show (pointer);
		}



		public override void Deserialize ()
		{
			base.Deserialize ();
			pointer = GetComponentInParent<PointerMono> ();
			//only null pointer on the first OnEnable deserialize for standalone objects
			if (pointer == null)
				return;
			if (pointer is StaticPointer && pointer.GetBoxedData () == null)
				pointer.Deserialize ();
		}

	}

	public enum RelationalType
	{
		IsGreaterThan,
		IsEqual,
	}


	public enum ArithmeticType
	{
		Assign,
		Add,
		Subtract,
		Multiply,
		Divide,
		Modulo,
		Average
	}

}