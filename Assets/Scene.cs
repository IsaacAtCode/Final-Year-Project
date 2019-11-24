using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
	private static bool created = false;

	private void Awake()
	{
		if (!created)
		{
			DontDestroyOnLoad(this.gameObject);
			created = true;

		}
	}




	public void GoToScene(string sceneName)
	{
		//possibly change to additive
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}
}
