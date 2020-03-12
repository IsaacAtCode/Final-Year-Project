using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.UI
{
	public class MainMenu : MonoBehaviour
	{

		private GameObject mainPanel;
		private GameObject trackPanel;
		private GameObject settingsPanel;

		enum MenuState
		{
			Main,
			TrackSelect, 
			Settings,
			Loading,
		}
		MenuState menuState;
		public int menuNum;

		public Animator anim;


		private void Start()
		{
			mainPanel = GameObject.Find("Main");
			trackPanel = GameObject.Find("Track");
			settingsPanel = GameObject.Find("Settings");

			ChangeMenu(0);
		}

		public void ChangeMenu(int newMenu)
		{
			menuNum = newMenu;
			anim.SetInteger("MenuState", menuNum);

			switch (menuNum)
			{
				case 0:
					menuState = MenuState.Main;
					break;
				case 1:
					menuState = MenuState.TrackSelect;
					break;
				case 2:
					menuState = MenuState.Settings;
					break;
			}
		}

		public void QuitGame()
		{
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}

	}
}
