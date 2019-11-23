using System.Collections.Generic;

namespace IsaacFagg
{
	public class Lap
	{ 
		public float time { get; set; }
		//Which lap in the race was this
		public int position { get; set; }
		//Where position the player was in the race (1st/2nd etc)
		public int playerPosition { get; set; }

		public List<Split> splits { get; set; }

		//Splits (Lapime between checkpoints, can be compared to other players)

		public Lap DeepCopy()
		{
			Lap dc = new Lap();

			dc.time = this.time;
			dc.position = this.position;
			dc.playerPosition = this.playerPosition;
			//Might be an issue
			dc.splits = this.splits;


			return dc;
		}
	}



	public class Split
	{
		public float time;
		public int position;
		public int playerPosition;
	}



}
