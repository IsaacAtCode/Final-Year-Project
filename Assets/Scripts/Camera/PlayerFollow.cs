using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
	public class PlayerFollow : MonoBehaviour
	{
		public GameObject playerGO;
		private Transform player;
		private Rigidbody2D rb;
		private Car cs;
		public float playerOffsetY;

		//Slight Drag to player
		public float damping;

		Vector3 velocity;

		private void Start()
		{
			playerGO = GameObject.FindGameObjectWithTag("Player");
			player = playerGO.GetComponent<Transform>();
			rb = playerGO.GetComponent<Rigidbody2D>();
			cs = playerGO.GetComponent<Car>();
		}

		private void FixedUpdate()
		{
			if (player == null)
			{
				return;
			}

			Vector3 targetPosition = new Vector3(player.position.x, player.position.y - playerOffsetY, -10);
			transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
		}

	}
}
