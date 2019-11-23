using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacFagg
{
	public class MainMenu : MonoBehaviour
	{
		private Canvas menuCanvas;

		private GameObject mainPanel;
		private GameObject trackPanel;
		private GameObject settingsPanel;

		enum MenuState
		{
			Main,
			TrackSelect, 
			Settings, 
			Quiting
		}
		MenuState menuState = MenuState.Main;
		public int menuNum;

		private void Start()
		{
			menuCanvas = GetComponentInChildren<Canvas>();
			mainPanel = GameObject.Find("Main");
			trackPanel = GameObject.Find("Track");
			settingsPanel = GameObject.Find("Settings");

			OpenMenu(mainPanel);
		}

		public void ChangeMenu(int newMenu)
		{
			menuNum = newMenu;

			switch (menuNum)
			{
				case 0:
					menuState = MenuState.Main;
					OpenMenu(mainPanel);
					Debug.Log(menuState);
					break;
				case 1:
					menuState = MenuState.TrackSelect;
					OpenMenu(trackPanel);

					Debug.Log(menuState);
					break;
				case 2:
					menuState = MenuState.Settings;
					OpenMenu(settingsPanel);

					Debug.Log(menuState);
					break;
				case 3:
					menuState = MenuState.Quiting;
					Debug.Log(menuState);
					QuitGame();
					break;
			}
		}

		private void HideAllMenus()
		{
			mainPanel.SetActive(false);
			trackPanel.SetActive(false);
			settingsPanel.SetActive(false);
		}

		private void OpenMenu(GameObject newMenu)
		{
			HideAllMenus();
			newMenu.SetActive(true);
		}

		private void QuitGame()
		{
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}

	}
}
