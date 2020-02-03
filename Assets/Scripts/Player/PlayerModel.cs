using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Player
{
	[RequireComponent(typeof(Player))]
	public class PlayerModel : MonoBehaviour
	{
		private Player player;

		private void Awake()
		{
			player = GetComponent<Player>();
		}

		public float Openness = 0f;
		public float Conscientiousness = 0f;
		public float Extraversion = 0f;
		public float Agreeableness = 0f;
		public float Neuroticism = 0f;










	}

}
