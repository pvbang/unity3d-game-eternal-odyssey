using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CountObjects : MonoBehaviour
{
    public static CountObjects Instance { get; private set; }
    public List<string> realtimeObjects = new List<string>();

    private readonly object _lock = new object(); // Khóa để đảm bảo thread-safe

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public bool Add(string realtimeObject)
    {
        lock (_lock) // Đảm bảo không bị truy cập đồng thời
        {
            if (!realtimeObjects.Contains(realtimeObject))
            {
                realtimeObjects.Add(realtimeObject);
                return true;
            }
        }
        return false;
    }

    public void Remove(string realtimeObject)
    {
        lock (_lock) // Đảm bảo không bị truy cập đồng thời
        {
            if (realtimeObjects.Contains(realtimeObject))
            {
                realtimeObjects.Remove(realtimeObject);
            }
        }
    }


    public int Count()
    {
        return realtimeObjects.Count;
    }


    // kiêm tra xem có tồn tại object này không
    public bool Contains(string realtimeObject)
    {
        lock (_lock) // Đảm bảo không bị truy cập đồng thời
        {
            return realtimeObjects.Contains(realtimeObject);
        }
    }
}
