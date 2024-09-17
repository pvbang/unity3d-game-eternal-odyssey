using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsManager : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();

    private void Awake()
    {
        //if (prefabs.Count == 0)
        //{
        //    prefabs.Add(gameObject);
        //}
    }

    // Lấy tất cả prefab
    public GameObject[] GetPrefabs()
    {
        return prefabs.ToArray();
    }

    // Lấy prefab theo tên
    public GameObject GetPrefab(string name)
    {
        foreach (GameObject prefab in prefabs)
        {
            if (prefab.name == name)
            {
                return prefab;
            }
        }

        return null;
    }

    // Lấy prefab theo index
    public GameObject GetPrefab(int index)
    {
        if (index >= 0 && index < prefabs.Count)
        {
            return prefabs[index];
        }

        return null; // Trả về null nếu index không hợp lệ
    }

    // Lấy prefab đầu tiên
    public GameObject GetPrefab()
    {
        if (prefabs.Count > 0)
        {
            return prefabs[0];
        }

        return null; // Trả về null nếu không có prefab nào
    }

    //////////////////////////////////////////////////////////////////////////

    // Set active cho tất cả prefab
    public void SetActiveAll(bool active)
    {
        foreach (GameObject prefab in prefabs)
        {
            prefab.SetActive(active);
        }
    }

    // Set active theo index
    public void SetActive(int index, bool active)
    {
        if (index >= 0 && index < prefabs.Count)
        {
            prefabs[index].SetActive(active);
        }
    }

    // Set active theo tên
    public void SetActive(string name, bool active)
    {
        foreach (GameObject prefab in prefabs)
        {
            if (prefab.name == name)
            {
                prefab.SetActive(active);
            }
        }
    }

    // Set active theo object
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
