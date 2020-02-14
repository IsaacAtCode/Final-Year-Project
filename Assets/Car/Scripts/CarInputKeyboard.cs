using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IsaacFagg.Cars
{
	public class CarInputKeyboard : CarInputBase
	{
		private void Update()
		{
			UpdateEnginePower();
			UpdateSteering();
		}

		void UpdateEnginePower()
		{
			if (Input.GetAxisRaw("Vertical") == -1)
			{
				SetEnginePower(-1);
			}
			else
			{
				SetEnginePower(1);
			}
			
		}

		void UpdateSteering()
		{
			SetSteeringDirection(-Input.GetAxisRaw("Horizontal"));
		}


	}
}
