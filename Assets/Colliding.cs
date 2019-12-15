using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliding : MonoBehaviour
{
	public float speed;

	public int currentTriggers;

	private void Update()
	{
		speed = CalcSpeed();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		currentTriggers++;

	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		currentTriggers--;
	}

	private float CalcSpeed()
	{
		if (currentTriggers == 2)
		{
			return 100;
		}
		else if (currentTriggers == 1)
		{
			//decay
			return 50;
		}
		else
		{
			return 0;
		}
	}

}
