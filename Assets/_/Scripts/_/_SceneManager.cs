using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class _SceneManager : MonoBehaviour
{
    [HideInInspector]
    public List<string> sceneNames = new();

    [HideInInspector]
    public string loadSceneName;


    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync(loadSceneName));
    }


    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Hiển thị Waiting UI để người chơi biết game đang tải
        WaitingManager.instance.ShowWaiting();

        // Bắt đầu load scene mới ở chế độ async
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        // Đảm bảo scene chỉ chuyển đổi sau khi load xong
        asyncLoad.allowSceneActivation = false;

        // Chờ đến khi scene mới load xong 90% trở lên
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Sau khi load xong, cho phép chuyển cảnh
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}
