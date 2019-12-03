using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DirtyGoToScene : MonoBehaviour
{
    Scene scene;

    public Scenes sceneToLoad;

    public enum Scenes
    {
        TestScene,
        MainMenu,
        Consent,
    }



    private void Start()
    {
        scene = Object.FindObjectOfType<Scene>();
        scene.GoToScene(sceneToLoad.ToString(), LoadSceneMode.Single);

    }

    


}

