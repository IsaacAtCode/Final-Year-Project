using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
	public Transform player;
	public float playerOffsetY;

	private void Update()
	{
		transform.position = new Vector3(player.position.x, player.position.y -playerOffsetY, -10);
	}
}
