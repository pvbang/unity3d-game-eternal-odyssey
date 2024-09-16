using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsManager : MonoBehaviour
{
    public GameObject[] prefabs;

    private void Awake()
    {
        if (prefabs.Length == 0)
        {
            prefabs[0] = gameObject;
        }
    }

    // Lấy tất cả prefab
    public GameObject[] GetPrefabs()
    {
        return prefabs;
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
        return prefabs[index];
    }

    // Lấy prefab đầu tiên
    public GameObject GetPrefab()
    {
        return prefabs[0];
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
        prefabs[index].SetActive(active);
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
