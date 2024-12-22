using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _instance;
    public static GameAssets Instance => _instance;

    public GameObject[] enemys;
    public GameObject[] items;

    // Sử dụng một dictionary để lưu trữ asset theo ID
    private Dictionary<string, GameObject> assetMap = new Dictionary<string, GameObject>();


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAssetMap();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAssetMap()
    {
        AddGameObjectsToMap(assetMap, enemys);
        AddGameObjectsToMap(assetMap, items);
    }

    private void AddGameObjectsToMap(Dictionary<string, GameObject> map, GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            ID idComponent = obj.GetComponent<ID>() ?? obj.GetComponentInChildren<ID>();
            string id = ((idComponent != null) && (idComponent.id != "")) ? idComponent.id : obj.name;

            if (!map.ContainsKey(id))
            {
                map.Add(id, obj);
            }
            else
            {
                Debug.LogWarning("ID trùng lặp: " + id + " cho GameObject: " + obj.name);
            }
        }
    }

    // Lấy gameobject từ ID
    // GameObject itemObject = GameAssets.Instance.GetGameObjectFromId(item.Key);
    public GameObject GetGameObjectFromId(string id)
    {
        if (assetMap.TryGetValue(id, out GameObject obj))
        {
            // Debug.Log("Assets: " + id);
            return obj;
        }
        else
        {
            Debug.LogWarning("Assets không tồn tại: " + id);
            return null;
        }
    }
}

