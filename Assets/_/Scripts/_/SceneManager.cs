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
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadSceneName);
    }

}
