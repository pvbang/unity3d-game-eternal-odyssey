using System.Collections.Generic;
using UnityEngine;


public class SceneManager : MonoBehaviour
{
    [HideInInspector]
    public List<string> sceneNames = new();

    [HideInInspector]
    public string loadSceneName;


    public void LoadScene()
    {
        WaitingManager.instance.ShowWaiting();
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadSceneName);
    }


    public static void LoadScene(string sceneName)
    {
        WaitingManager.instance.ShowWaiting();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

}
