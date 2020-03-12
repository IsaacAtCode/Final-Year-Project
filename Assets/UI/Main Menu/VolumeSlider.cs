using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace IsaacFagg.UI.Main
{
	public class VolumeSlider : MonoBehaviour
	{
		public float volume;

		public Slider vSlider;
		public InputField vInput;

		public AudioMixer mixer;

		private void Start()
		{
			vSlider = GetComponentInChildren<Slider>();
			vInput = GetComponentInChildren<InputField>();

			vSlider.value = PlayerPrefs.GetFloat("Volume", 0.75f);
		}

		public void OnInputEdit()
		{
			float inputValue = Mathf.Clamp(float.Parse(vInput.text)/100, 0.0001f, 1.0f);
			vSlider.value = inputValue;
		}

		public void SetInput()
		{
			int inputValue;

			if (vSlider.value < 0.01f)
			{
				inputValue = 0;
			}
			else
			{
				inputValue = Mathf.CeilToInt(vSlider.value * 100);
			}


			vInput.text = inputValue.ToString();
		}


		public void SetLevel(float sliderValue)
		{
			mixer.SetFloat("Volume", Mathf.Log10(sliderValue) * 20);
			PlayerPrefs.SetFloat("Volume", sliderValue);
		}


	}
}
