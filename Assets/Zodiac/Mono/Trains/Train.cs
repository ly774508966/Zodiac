using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
	[Range (0, 1)]
	public float speed = 0.1f;
	public Track startTrack;

	Track currentTrack;
	float trackProgress;


}
