using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	public int location;
	public bool finishLine;
	public Transform position;


	private void Start()
	{
		position = this.transform;
	}


}
