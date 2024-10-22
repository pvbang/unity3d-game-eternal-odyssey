using System;
using System.Linq;
using UnityEngine;

public class TimeManager
{
    public static string Now()
    {
        return DateTime.Now.ToString("yyyy-MM-dd.HH:mm:ss:fff");
    }
}
