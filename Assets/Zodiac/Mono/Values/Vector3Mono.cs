using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;



namespace Zodiac.Mono
{

	public class Vector3Mono : PropertyMono<Vector3,Vector3Mono>
	{

		public float dataX{ get { return data.x; } set { setCallback (new Vector3 (value, data.y, data.z)); } }

		public float dataY{ get { return data.y; } set { setCallback (new Vector3 (data.x, value, data.z)); } }

		public float dataZ{ get { return data.z; } set { setCallback (new Vector3 (data.x, data.y, value)); } }


		//this could be a type parameter for property monos
		public List<FloatMono> FloatFactors;


		public override void Init ()
		{
			FloatFactors = new List<FloatMono> ();
			base.Init ();
		}

		public override bool GetRelationalResult ()
		{
			switch (relationalType) {
			case RelationalType.IsEqual:
				return arithmeticFactors.All (v => v.data == data);
			case RelationalType.IsGreaterThan:
				return arithmeticFactors.All (v => v.data.magnitude < data.magnitude);
			}
			return true;
		}

		public override void Connect (IEnumerable<ZodiacMono> _selected)
		{
			FloatFactors.AddRange (_selected.Where (s => s is FloatMono).Cast<FloatMono> ());
			base.Connect (_selected);
		}

		public override void PerformArithmetic ()
		{
			if (arithmeticFactors == null || !arithmeticFactors.Any ())
				return;
			base.PerformArithmetic ();
			Vector3 val = Vector3.zero;//getCallback ();
			switch (arithmeticType) {
			case ArithmeticType.Add:
				arithmeticFactors.ForEach (v => val += v.data);
				setCallback (val);
				return;
			case ArithmeticType.Subtract:
				val = arithmeticFactors.First ().data * 2;
				arithmeticFactors.ForEach (v => val -= v.data);
				//arithmeticFactors.ForEach (v => Debug.Log (v.data));
				//Debug.Log (val);
				setCallback (val);
				return;
			case ArithmeticType.Multiply:
				val = Vector3.one;
				foreach (var v in arithmeticFactors) {
					val.x *= v.data.x;
					val.y *= v.data.y;
					val.z *= v.data.z;
				}
				setCallback (val);
				return;
			case ArithmeticType.Divide:
				val = Vector3.one;
				foreach (var v in arithmeticFactors) {
					val.x /= v.data.x;
					val.y /= v.data.y;
					val.z /= v.data.z;
				}
				setCallback (val);
				return;
			}
		}
	}
}