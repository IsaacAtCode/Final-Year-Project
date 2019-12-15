using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
	public void GoToScene(string sceneName, LoadSceneMode loadSceneMode)
	{
		//possibly change to additive
		SceneManager.LoadScene(sceneName, loadSceneMode);
	}
}
