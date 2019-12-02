using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace IsaacFagg.Settings
{
	public class Volume : MonoBehaviour
	{
		public float defaultValue = 0.5f;
		public static float currentValue;

		public Slider volumeSlider;
		public InputField volumeInputField;

		public AudioMixer mixer;

		private void Start()
		{
			Debug.Log("Starting Value = " + currentValue);


			volumeSlider = GetComponentInChildren<Slider>();
			volumeInputField = GetComponentInChildren<InputField>();

			SetCurrentVolume();

		}

		private void GetVolume()
		{
			Debug.Log("Before Getting " + currentValue);

			//If the player has played before, it gets the value for the 
			//if (PlayerPrefs.HasKey(this.gameObject.name))
			//{
			//	currentValue = PlayerPrefs.GetFloat(this.gameObject.name);
			//	Debug.Log("Got " + currentValue);

			//}
			////if it doesnt exist, make it and then assign it the default value
			//else if (!PlayerPrefs.HasKey(this.gameObject.name))
			//{
			//	PlayerPrefs.SetFloat(this.gameObject.name, defaultValue);
			//	currentValue = PlayerPrefs.GetFloat(this.gameObject.name);
			//	Debug.Log("Got/default " + currentValue);
			//}

			currentValue = PlayerPrefs.GetFloat(this.gameObject.name);

			Debug.Log("After Getting " + currentValue);


		}

		public void SetCurrentVolume()
		{
			GetVolume();

			volumeInputField.text = (Mathf.RoundToInt(currentValue * 100)).ToString();
			volumeSlider.value = currentValue;
		}

		public void SetVolume()
		{
			PlayerPrefs.SetFloat(this.gameObject.name, currentValue);
		}

		public void SetSliderValue()
		{
			//if (float.Parse(volumeInputField.text) < 0)
			//{
			//	currentValue = 0;
			//}
			//else if (float.Parse(volumeInputField.text) > 100)
			//{
			//	volumeInputField.textComponent.text = "100";
			//}
			
			currentValue = float.Parse(volumeInputField.text) / 100;

			volumeSlider.value = currentValue;
			SetVolume();
		}

		public void SetInputFieldValue()
		{
			volumeInputField.text = (Mathf.RoundToInt(volumeSlider.value * 100)).ToString();
			currentValue = Mathf.RoundToInt(volumeSlider.value * 100);
			SetVolume();
		}

		public void SetLevel(float sliderValue)
		{
			mixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);
		}


	}
}
