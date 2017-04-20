using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Zodiac.Mono;

namespace Zodiac
{
	[RequireComponent (typeof(LineRenderer))]
	public class LinkRenderer : MonoBehaviour
	{


		public static LinkRenderer AddLinkRenderer (ZodiacMono mono, LinkerType linkerType)
		{
			GameObject go = new GameObject ();
			go.transform.parent = mono.transform;
			var linkRenderer = go.AddComponent<LinkRenderer> ();
			linkRenderer.Init (linkerType);
			return linkRenderer;
		}

		[SerializeField]
		LineRenderer lineRenderer;

		public void Init (LinkerType linkerType)
		{
			lineRenderer = GetComponent<LineRenderer> ();
			lineRenderer.SetWidth (0.05f, 0.05f);
			lineRenderer.useWorldSpace = true;
			SetColor (linkerType);
			lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
			name = LinkerTypeToString (linkerType);
		}

		public void Show <T> (List<T> _targets) where T : Component
		{
			Show (_targets.Where (tar => tar != null).Select (tar => tar.transform));
		}

		public void Show (IEnumerable<Transform> _targets)
		{
			var positionsList = new List<Vector3> ();
			positionsList.Add (transform.parent.position);
			foreach (var target in _targets) {
				positionsList.Add (target.position);
				positionsList.Add (transform.parent.position);
			}
			SetLineRenderer (positionsList);
		}

		public void Show (ValueMono _target)
		{
			Show (_target.transform);
		}

		public void Show (Transform _target)
		{
			var positionsList = new List<Vector3> ();
			positionsList.Add (transform.parent.position);
			if (_target != null) {
				positionsList.Add (_target.position);
				//there and back for same effect as multiple targets
				positionsList.Add (transform.parent.position);
			}
			SetLineRenderer (positionsList);
		}

		void SetLineRenderer (List<Vector3> positionsList)
		{

			lineRenderer.SetVertexCount (positionsList.Count);
			lineRenderer.SetPositions (positionsList.ToArray ());

		}

		string LinkerTypeToString (LinkerType type)
		{
			string name = "";
			switch (type) {
			case LinkerType.ArithmeticFactor:
				name = "Arithmetic";
				break;
			case LinkerType.Parameter:
				name = "Parameter";
				break;
			case LinkerType.Pointer:
				name = "Pointer";
				break;
			}
			return name + " Linker";
		}


		void SetColor (LinkerType linkerType)
		{
			Color col = Color.white;
			switch (linkerType) {
			case LinkerType.ArithmeticFactor:
				col = Color.green;
				break;
			case LinkerType.Parameter:
				col = Color.yellow;
				break;
			case LinkerType.Pointer:
				col = Color.blue;
				break;
			case LinkerType.Trigger:
				col = Color.magenta;
				break;
			}
			lineRenderer.SetColors (col, col);
		}

	}

	public enum LinkerType
	{
		ArithmeticFactor,
		Parameter,
		Pointer,
		Trigger
	}


}