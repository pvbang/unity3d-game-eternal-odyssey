using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Cách dùng: StartCoroutine(CoroutineHelper.DelaySeconds(() => , 1f));

public static class CoroutineHelper
{
    // hàm này sẽ delay một hàm nào đó
    public static IEnumerator DelaySeconds(System.Action action, float delay)
    {
        //Debug.Log("Delay: " + delay);
        yield return new WaitForSeconds(delay);
        action();
    }
}