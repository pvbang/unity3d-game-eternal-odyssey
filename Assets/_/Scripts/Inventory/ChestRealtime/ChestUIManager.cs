using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUIManager : MonoBehaviour
{
    public GameObject chestPrefab;
    private Dictionary<string, GameObject> spawnedChests = new Dictionary<string, GameObject>();
    private Dictionary<string, ItemRealtime> listChests = new Dictionary<string, ItemRealtime>();
    public Dictionary<string, ItemRealtime> ListChests => listChests;
    private static ChestUIManager _instance;
    public static ChestUIManager Instance => _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    public void Start()
    {
        StartListeningToChestChanges();
        StartCoroutine(LoadAndDisplayAllChestsCoroutine());
    }

    public void CreateNewChest(Vector3 position, string itemId, int quantity, System.Action<bool> callback)
    {
        string chestId = System.Guid.NewGuid().ToString();
        MaterialsUpdate newItem = new MaterialsUpdate(itemId, quantity);

        ItemRealtimeManager.CreateChestAtPosition(chestId, position, itemId, newItem, (success) =>
        {
            if (success)
            {
                callback?.Invoke(true);
            }
            else
            {
                Debug.LogError("Lỗi khi tạo rương mới.");
                callback?.Invoke(false);
            }
        });
    }
    public void DisplayChest(ItemRealtime chestData)
    {
        if (spawnedChests.ContainsKey(chestData.chestId))
        {
            Debug.LogWarning($"Rương với ID {chestData.chestId} đã tồn tại trong scene.");
            return;
        }
        Vector3 chestPosition = new Vector3(chestData.location.x, chestData.location.y, chestData.location.z);
        GameObject chestInstance = Instantiate(chestPrefab, chestPosition, Quaternion.identity);
        chestInstance.name = $"{chestData.chestId}";
        spawnedChests[chestData.chestId] = chestInstance;
    }

    public void RemoveChest(string chestId)
    {
        if (spawnedChests.TryGetValue(chestId, out GameObject chestInstance))
        {
            Destroy(chestInstance);
            spawnedChests.Remove(chestId);
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy rương với ID {chestId} để xóa.");
        }

        if (listChests.ContainsKey(chestId))
        {
            listChests.Remove(chestId);
        }
    }

    public IEnumerator LoadAndDisplayAllChestsCoroutine()
    {
        bool isDataLoaded = false;
        List<ItemRealtime> allChests = null;
        ItemRealtimeManager.LoadAllChests((loadedChests) =>
        {
            allChests = loadedChests;
            isDataLoaded = true;
        });
        yield return new WaitUntil(() => isDataLoaded);

        if (allChests != null && allChests.Count > 0)
        {
            foreach (var chest in allChests)
            {
                if (!listChests.ContainsKey(chest.chestId))
                {
                    listChests[chest.chestId] = chest;
                }
            }
            foreach (var chest in allChests)
            {
                DisplayChest(chest);
                yield return new WaitForSeconds(0.5f);
            }
        }
        else
        {
            Debug.LogWarning("Không có rương nào để hiển thị.");
        }
    }

    public void StartListeningToChestChanges()
    {
        ItemRealtimeManager.ListenToChestChanges(
            (newChest, chestId) =>
            {
                DisplayChest(newChest);
            },
            (removedChestId) =>
            {
                RemoveChest(removedChestId);
            });
    }

}
