using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg
{
	public class PlayerFollow : MonoBehaviour
	{
		public Transform player;
		public float playerOffsetY;

		//Slight Drag to player

		private void Update()
		{
			transform.position = new Vector3(player.position.x, player.position.y - playerOffsetY, -10);
		}
	}
}
