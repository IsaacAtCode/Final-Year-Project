using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
	public class PlayerFollow : MonoBehaviour
	{
		public GameObject playerGO;
		private Vector3 playerPos;
		public float offset;

		//Slight Drag to player
		public float damping;

		private void FixedUpdate()
		{
			if (playerGO == null)
			{
				playerGO = GameObject.FindGameObjectWithTag("Player");
			}

			playerPos = playerGO.transform.position;

			Vector3 targetPosition = new Vector3(playerPos.x, playerPos.y, -offset);
			transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
		}

	}
}
