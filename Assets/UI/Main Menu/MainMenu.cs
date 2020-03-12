using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg.UI.Main
{
	public class MainMenu : MonoBehaviour
	{

		public GameObject mainPanel;
		public GameObject trackPanel;
		public GameObject settingsPanel;
		public GameObject playerPanel;

		public MenuState menuState;

		private void Start()
		{

			ChangeMenu(0);
		}

		public void ChangeMenu(int newMenu)
		{
			int menuNum = newMenu;

			switch (menuNum)
			{
				case 0:
					menuState = MenuState.Main;
					OpenMain();
					break;
				case 1:
					menuState = MenuState.TrackSelect;
					OpenTrack();
					break;
				case 2:
					menuState = MenuState.Settings;
					OpenSettings();
					break;
				case 3:
					menuState = MenuState.Player;
					OpenPlayer();
					break;
			}
		}

		private void OpenMain()
		{
			mainPanel.SetActive(true);
			trackPanel.SetActive(false);
			settingsPanel.SetActive(false);
			playerPanel.SetActive(false);
		}

		private void OpenTrack()
		{
			mainPanel.SetActive(false);
			trackPanel.SetActive(true);
			settingsPanel.SetActive(false);
			playerPanel.SetActive(false);
		}

		private void OpenSettings()
		{
			mainPanel.SetActive(false);
			trackPanel.SetActive(false);
			settingsPanel.SetActive(true);
			playerPanel.SetActive(false);
		}

		private void OpenPlayer()
		{
			mainPanel.SetActive(false);
			trackPanel.SetActive(false);
			settingsPanel.SetActive(false);
			playerPanel.SetActive(true);
		}


		public void QuitGame()
		{
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else

				//SAVE GAME
				Application.Quit();
			#endif
		}

	}

	public enum MenuState
	{
		Main,
		TrackSelect,
		Settings,
		Player,
		Loading,
	}
}
