using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsManager : MonoBehaviour
{
    public GameObject[] prefabs;

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

    public GameObject GetPrefab(int index)
    {
        return prefabs[index];
    }

    public GameObject GetPrefab()
    {
        // Return the first prefab in the list
        return prefabs[0];
    }
}
