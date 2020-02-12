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
			SetEnginePower(Input.GetAxisRaw("Vertical"));
		}

		void UpdateSteering()
		{
			SetSteeringDirection(-Input.GetAxisRaw("Horizontal"));
		}


	}
}
