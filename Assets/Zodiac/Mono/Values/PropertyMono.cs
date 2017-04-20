using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
using System.Reflection;
using System.Linq;

//methinks this will become a generic class with vector3s, floats and pointers all using the same system
using Object = UnityEngine.Object;

namespace Zodiac.Mono
{
	
	public abstract class PropertyMono<Tdata,Tthis> : MemberMono where Tthis : MemberMono
	{

		//should this be a virtual property and child class standalone float has an override of the get and set methods?
		public Tdata data {
			get {
				PerformArithmetic ();
				return getCallback ();
			}
			set { setCallback (value); }
		}


		//this will be ignored if data is pointing to another object
		public Tdata dataProperty{ get { return dataField; } set { dataField = value; } }

		public Tdata dataField;

		public Func<Tdata> getCallback;
		public Action<Tdata> setCallback;

		public MemberMono assignmentFactor;
		public List<Tthis> arithmeticFactors;

		[SerializeField]
		LinkRenderer arithmeticLinker;
		[SerializeField]
		LinkRenderer assignmentLinker;

		public override void Init ()
		{
			arithmeticFactors = new List<Tthis> ();
			arithmeticLinker = LinkRenderer.AddLinkRenderer (this, LinkerType.ArithmeticFactor);
			assignmentLinker = LinkRenderer.AddLinkRenderer (this, LinkerType.ArithmeticFactor);
			base.Init ();
		}


		void StaticPointerDeserialize (Type typePointer)
		{
			getCallback = (getCallbackName == "") ? null : (Func<Tdata>)Delegate.CreateDelegate (typeof(Func<Tdata>), typePointer, getCallbackName);
			setCallback = (setCallbackName == "") ? null : (Action<Tdata>)Delegate.CreateDelegate (typeof(Action<Tdata>), typePointer, setCallbackName);
		}

		void ObjectPointerDeserialize (UnityEngine.Object objPointer)
		{
			getCallback = (getCallbackName == "") ? null : (Func<Tdata>)Delegate.CreateDelegate (typeof(Func<Tdata>), objPointer, getCallbackName);
			setCallback = (setCallbackName == "") ? null : (Action<Tdata>)Delegate.CreateDelegate (typeof(Action<Tdata>), objPointer, setCallbackName);
		}

		public override void Connect (IEnumerable<ZodiacMono> _selected)
		{
			base.Connect (_selected);
			arithmeticFactors.AddRange (_selected.Where (s => s is Tthis).Cast<Tthis> ());
			if (_selected.Any (s => s is MemberMono))
				assignmentFactor = (MemberMono)_selected.First (s => s is MemberMono);
		}

		public override object GetBoxedData ()
		{
			if (getCallback == null)
				Deserialize ();
			return ((object)getCallback ());
		}

		protected override void Update ()
		{
			assignmentLinker.Show (assignmentFactor);
			arithmeticLinker.Show<Tthis> (arithmeticFactors);
			PerformArithmetic ();
			base.Update ();
		}

		public virtual void PerformArithmetic ()
		{
			switch (arithmeticType) {
			case ArithmeticType.Assign:
				if (assignmentFactor == null)
					return;
				var val = assignmentFactor.GetBoxedData ();
				if (val is Tdata)
					setCallback ((Tdata)val);
				return;
			}
		}

		public override void SetBoxedData (object obj)
		{
			// i know i know, whats the syntax?
			var tempList = new List<object> ();
			tempList.Add (obj);
			setCallback (tempList.Cast<Tdata> ().First ());
		}




		public override void Deserialize ()
		{
			base.Deserialize ();
			if (pointer == null)
				return;
			if (pointer.GetBoxedData () == null)
				return;
			if (pointer is StaticPointer) {
				StaticPointerDeserialize ((Type)pointer.GetBoxedData ());
			} else if (pointer is UnityObjectPointer) {
				ObjectPointerDeserialize ((Object)pointer.GetBoxedData ());
			}
		}
			
	}

}

