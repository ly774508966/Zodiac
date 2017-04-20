using UnityEngine;
using System.Collections;


namespace Zodiac
{
	public static class Zodiac
	{

		public static Transform GetZodiacRoot ()
		{
			var zodiacRoot = GameObject.FindGameObjectWithTag ("Zodiac Root");
			if (zodiacRoot == null) {
				zodiacRoot = new GameObject ("Zodiac");
				zodiacRoot.transform.position = Vector3.up * 2;
				zodiacRoot.transform.tag = "Zodiac Root";
				zodiacRoot.AddComponent<ZodiacCreator> ();
				zodiacRoot.AddComponent<ZodiacConnector> ();

			}
			return zodiacRoot.transform;
		}

	}
}