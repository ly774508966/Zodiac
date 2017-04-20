using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;


using Zodiac.Scriptable;

namespace Zodiac.Mono
{
	public class MethodMono : MemberMono
	{
		public ScriptableMethod scriptableMethod;

		public List<ValueMono> parameters;

		//made private, shouldnt break
		object methodCallback;


		public bool deserialize;

		public bool invoke;

		[SerializeField]
		LinkRenderer parameterLinker;

		protected override void OnValidate ()
		{
			base.OnValidate ();
			if (deserialize) {
				Deserialize ();
				deserialize = false;
			}
			if (invoke) {
				GetBoxedData ();
				invoke = !invoke;
			}

		}

		public void Init (ScriptableMethod _scriptableMethod, PointerMono _pointer)
		{
			parameters = new List<ValueMono> ();
			scriptableMethod = _scriptableMethod;
			dataType = scriptableMethod.returnType.children.First ().name;
			getCallbackName = scriptableMethod.name;
			parameterLinker = LinkRenderer.AddLinkRenderer (this, LinkerType.Parameter);
			base.Init (_pointer);
			name += " " + getCallbackName;
		}

		protected override void Update ()
		{
			base.Update ();
			parameterLinker.Show (parameters);
		}


		public override void Connect (IEnumerable<ZodiacMono> _selected)
		{
			var valueMonos = _selected.Where (s => s is ValueMono).Cast<ValueMono> ();
			parameters.AddRange (valueMonos.Where (v => !parameters.Contains (v)));
		}

		//this will attempt to bind the method
		//if parameters are not suitable or not in a suitable order an exception will be thrown
		public override void Deserialize ()
		{
			base.Deserialize ();
			//only null pointer on the first OnEnable deserialize for standalone objects
			if (pointer == null)
				return;

			Type delegateType;
			Type[] paramTypes = new Type[0];
			paramTypes = (parameters == null) ? paramTypes : parameters.Select (p => p.GetBoxedData ().GetType ()).ToArray ();
			//paramTypes.ToList ().ForEach (p => Debug.Log (p.Name));
			if (Type.GetType (dataType) == typeof(void)) {
				delegateType = Expression.GetActionType (paramTypes);
			} else {
				var returnAndParamTypes = new Type[paramTypes.Length + 1];
				returnAndParamTypes [0] = Type.GetType (dataType);
				for (int i = 0; i < paramTypes.Length; i++) {
					returnAndParamTypes [i + 1] = paramTypes [i];
				}
				paramTypes = returnAndParamTypes;
				delegateType = Expression.GetFuncType (returnAndParamTypes);
			}
			try {
				if (pointer is StaticPointer) {
					//coulnt get delegates goin for static pointers, using methodinfo invoke instead
					//Debug.Log (pointer.GetPointerType ().ToString () + getCallbackName);
					//Debug.Log (methodInfo.ToString ());
					//methodCallback = Delegate.CreateDelegate (delegateType, methodInfo);
					//methodCallback = Delegate.CreateDelegate (delegateType, pointer.GetPointerType (), getCallbackName, true, true);
				} else if (pointer is UnityObjectPointer) {
					methodCallback = Delegate.CreateDelegate (delegateType, pointer.GetBoxedData (), getCallbackName);
				}
			} catch {
				Debug.LogError ("Method binding error, check parameters are suitable", transform);
			}
		}

		public override object GetBoxedData ()
		{
			var parameterValues = parameters.Select (v => v == null ? null : v.GetBoxedData ()).ToArray ();
			object result = null;
			try {
				if (pointer is StaticPointer) {
					result = pointer.GetPointerType ().GetMethod (getCallbackName).Invoke (null, parameterValues);
				} else {
					return ((Delegate)methodCallback).DynamicInvoke (parameterValues);
				}
			} catch {
				Debug.LogError ("Method Invoking error, check parameters are suitable", transform);
			}
			return result;

		}



	}
}