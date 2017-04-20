using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Object = UnityEngine.Object;


namespace Zodiac.Scriptable
{

	public class ScriptableHeader : ScriptableBase
	{

		public bool foldout;
		public Vector2 scrollPos;

		public List<ScriptableBase> children;

		public void Init (string _name)
		{
			name = _name;
			children = new List<ScriptableBase> ();
		}

		static ScriptableHeader CheckEmpty (ScriptableHeader header)
		{
			return (header.children.Count == 0) ? null : header;
		}

		public static ScriptableHeader CreateEmptyHeader (string _name)
		{
			var header = ScriptableObject.CreateInstance<ScriptableHeader> ();
			header.Init (_name);
			return header;
		}

		//this will return null if the children list is empty
		public static ScriptableHeader CreateHeader (string _name, List<ScriptableBase> _children)
		{
			var header = ScriptableObject.CreateInstance<ScriptableHeader> ();
			header.name = _name;
			header.children = _children;
			return CheckEmpty (header);
		}


	}
}