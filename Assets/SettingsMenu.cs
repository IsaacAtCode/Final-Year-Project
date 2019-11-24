using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.UI
{
	public class SettingsMenu : MonoBehaviour
	{
		[Header("Volume")]
		public Slider generalSlider;
		public Slider musicSlider;
		public Slider sfxSlider;
		public Slider engineSlider;
		public InputField generalText;
		public InputField musicText;
		public InputField sfxText;
		public InputField engineText;

	}
}
