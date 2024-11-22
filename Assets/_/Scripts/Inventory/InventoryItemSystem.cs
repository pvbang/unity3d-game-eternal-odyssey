using Firebase.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemSystem : MonoBehaviour
{
    public List<Inventory> inventory = new List<Inventory>(); 
    public Dictionary<string, Item> itemDetails = new Dictionary<string, Item>();
    public Action onInventoryUpdated; 

    public static InventoryItemSystem instance;
    public List<Inventory> groundItems = new List<Inventory>();

    public bool isDataLoaded = false;
    public string userId;
    public string worldId;
    private void Awake()
    {
        worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        userId = FirebaseManager.instance.GetUserId();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadItemDetails(() =>
        {
            LoadInventoryFromFirebase(inventoryList =>
            {
                Debug.Log("Tổng số item trong inventory: " + inventoryList.Count);
                // Tạo một danh sách tạm để lưu các mục đã hoàn thiện
                List<Inventory> completedInventoryList = new List<Inventory>();

                foreach (var inventoryItem in inventoryList)
                {
                    Debug.Log($"Item ID: {inventoryItem.itemId}, Quantity: {inventoryItem.quantity}");
                    if (instance.itemDetails.TryGetValue(inventoryItem.itemId, out Item itemDetail))
                    {
                        Inventory completeInventory = new Inventory(itemDetail, inventoryItem.quantity);
                        completedInventoryList.Add(completeInventory);
                    }
                    else
                    {
                        Debug.LogWarning($"Không tìm thấy thông tin chi tiết cho item với ID: {inventoryItem.itemId}");
                    }
                }
                this.inventory = completedInventoryList;
                onInventoryUpdated?.Invoke();
            });
        });
    }

    // Hàm lấy thông tin chi tiết của Inventory từ Firebase
    public void LoadInventoryFromFirebase(Action<List<Inventory>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId)
            .Child("playerWorld");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy danh sách inventory: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<Inventory> inventoryList = new List<Inventory>();

                Debug.Log($"Tổng số user: {snapshot.ChildrenCount}"+ "WorldID:"+ worldId);

                foreach (var child in snapshot.Children)
                {
                    PlayerWorld playerworld = JsonUtility.FromJson<PlayerWorld>(child.GetRawJsonValue());

                    Debug.Log($"User ID: {playerworld.userId}"+ userId);

                    if (playerworld.userId == userId)
                    {
                        inventoryList = playerworld.inventorys;
                        break; 
                    }  
                }

                if (inventoryList == null || inventoryList.Count == 0)
                {
                    Debug.LogWarning("Không tìm thấy inventory hoặc inventory trống.");
                }
                callback(inventoryList);  // Trả về danh sách inventory qua callback
            }
        });
    }

    // Phương thức để tải thông tin chi tiết của các item từ Firebase
    public void LoadItemDetails(Action callback)
    {
        ItemManager.GetItems(items =>
        {
            if (items == null)
            {
                Debug.LogError("Không thể tải thông tin items từ Firebase.");
                return;
            }

            // Lưu các item vào itemDetails dictionary
            foreach (var item in items)
            {
                if (!itemDetails.ContainsKey(item.itemId))
                {
                    itemDetails.Add(item.itemId, item);
                }
            }

            isDataLoaded = true;
            callback?.Invoke();
            Debug.Log("Đã tải xong tất cả các item từ Firebase.");
        });
    }

    // Thêm vật phẩm vào Inventory từ Ground
    public void AddItemToInventory(string itemId, int quantity)
    {
        var groundItem = groundItems.Find(i => i.item.itemId == itemId);
        if (groundItem != null)
        {
            var existingItem = inventory.Find(i => i.item.itemId == itemId);

            if (existingItem != null)
            {
                existingItem.quantity += quantity;
            }
            else
            {
                inventory.Add(new Inventory(groundItem.item, quantity));
            }
            groundItem.quantity -= quantity;
            if (groundItem.quantity <= 0)
            {
                groundItems.Remove(groundItem);
            }

            // Cập nhật lại UI
            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
            {
                inventoryUI.UpdateInventoryUI(inventory);
            }
        }
    }

    // Xóa vật phẩm khỏi Inventory và thêm vào Ground
    public void RemoveItemFromInventory(string itemId, int quantityToRemove)
    {
        var inventoryItem = inventory.Find(i => i.item.itemId == itemId);
        if (inventoryItem != null)
        {
            inventoryItem.quantity -= quantityToRemove;

            if (inventoryItem.quantity <= 0)
            {
                inventory.Remove(inventoryItem);
            }

            // Cập nhật lại UI Inventory
            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
            {
                inventoryUI.UpdateInventoryUI(inventory);
            }

        }
    }
    public void UpdateItemQuantity(string itemId, int newQuantity)
    {
        var inventoryItem = inventory.Find(i => i.item.itemId == itemId);
        if (inventoryItem != null)
        {
            inventoryItem.quantity = newQuantity;
            onInventoryUpdated?.Invoke();
        }
    }

}
