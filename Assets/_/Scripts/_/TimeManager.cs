using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimeManager
{
    public static string Now()
    {
        return DateTime.Now.ToString("yyyy-MM-dd.HH:mm:ss:fff");
    }

    public static IEnumerator DelaySeconds(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
