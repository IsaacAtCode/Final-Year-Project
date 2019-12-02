using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
	public void GoToScene(string sceneName)
	{
		//possibly change to additive
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}
}
