using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
	public class PlayerFollow : MonoBehaviour
	{
		public GameObject playerGO;
		private Transform playerTransform;
		public float offset;

		//Slight Drag to player
		public float damping;

		private void Start()
		{
			if (playerGO == null)
			{
				playerGO = GameObject.FindGameObjectWithTag("Player");
			}
			playerTransform = playerGO.GetComponent<Transform>();

			Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, -offset);
			transform.position = targetPosition;

		}

		private void FixedUpdate()
		{
			if (playerTransform == null)
			{
				return;
			}

			Vector3 targetPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, -offset);
			transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
		}

	}
}
