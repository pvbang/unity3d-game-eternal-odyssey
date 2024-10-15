using Firebase.Database;
using NullSave.TOCK.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemSystem : MonoBehaviour
{
    public List<Inventory> inventory = new List<Inventory>();  // Danh sách các vật phẩm trong kho
    public Dictionary<string, Item> itemDetails = new Dictionary<string, Item>();  // Lưu trữ chi tiết thông tin vật phẩm
    public Action onInventoryUpdated;  // Delegate để cập nhật UI hoặc xử lý logic khi Inventory được tải xong

    public static InventoryItemSystem instance;  // Singleton cho InventoryItemSystem
    public List<Inventory> groundItems = new List<Inventory>();  // Danh sách các vật phẩm trên mặt đất
    void Start()
    {
        // Lấy worldId và userId từ GameManager
        string worldId = "AU0NOXSIN7BZ9PQ1GL6XUZDSYHWB9O";
        string userId = "zpNXdXDhdPRkzY3nUtV7VqnHK3S2";

        if (!string.IsNullOrEmpty(worldId) && !string.IsNullOrEmpty(userId))
        {
            LoadInventoryDetailsFromFirebase(worldId, userId);
        }
        else
        {
            Debug.LogError("World ID hoặc User ID không hợp lệ.");
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Thêm vật phẩm vào Inventory
    public void AddItem(string worldId, string userId, string idItem, int quantity)
    {
        // Tìm item trong danh sách hiện tại
        Inventory item = inventory.Find(i => i.itemId == idItem);
        if (item != null)
        {
            item.quantity += quantity;
        }
        else
        {
            inventory.Add(new Inventory { itemId = idItem, quantity = quantity });
        }

        // Tự động lấy thông tin chi tiết nếu chưa có trong Dictionary
        if (!itemDetails.ContainsKey(idItem))
        {
            ItemManager.GetItemById(idItem, (itemData) =>
            {
                if (itemData != null)
                {
                    itemDetails.Add(idItem, itemData);
                    onInventoryUpdated?.Invoke();  // Gọi delegate cập nhật UI khi tải xong
                }
            });
        }
        else
        {
            onInventoryUpdated?.Invoke();  // Gọi delegate cập nhật UI nếu đã có thông tin
        }
    }


    // Xóa vật phẩm khỏi Inventory
    public void RemoveItem(string itemInventoryId, int Quantity)
    {
        Inventory item = inventory.Find(i => i.itemId == itemInventoryId);
        if (item != null)
        {
            item.quantity -= Quantity;
            if (item.quantity <= 0)
                inventory.Remove(item);
        }
    }

    // Hàm lấy thông tin chi tiết của Inventory
    public void LoadInventoryDetailsFromFirebase(string worldId, string userId)
    {
        // Đường dẫn tới Inventory của người chơi
        var inventoryRef = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId)
            .Child("userId")
            .Child(userId)
            .Child("inventory");

        Debug.Log($"Đang tải chi tiết thông tin inventory từ world: {worldId}, user: {userId}, inventory: {inventoryRef}");

        // Lấy dữ liệu Inventory từ Firebase
        inventoryRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Lỗi khi tải dữ liệu Inventory từ Firebase: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                inventory.Clear();

                foreach (DataSnapshot itemSnapshot in snapshot.Children)
                {
                    // Lấy `itemId` và `quantity` từ Inventory của người chơi
                    string itemId = itemSnapshot.Child("itemId").Value.ToString();
                    int quantity = int.Parse(itemSnapshot.Child("quantity").Value.ToString());

                    // Thêm vào danh sách Inventory
                    inventory.Add(new Inventory(itemId, quantity));
                    Debug.Log($"Lấy item {itemId} từ Inventory với số lượng: {quantity}");
                }

                // Sau khi có danh sách Inventory, tải thông tin chi tiết từng Item
                LoadItemDetails();
            }
        });
    }
    private void LoadItemDetails()
    {
        foreach (var playerItem in inventory)
        {
            if (!itemDetails.ContainsKey(playerItem.itemId))
            {
                ItemManager.GetItemById(playerItem.itemId, (itemData) =>
                {
                    if (itemData != null)
                    {
                        itemDetails.Add(playerItem.itemId, itemData);
                        Debug.Log($"Đã lấy thông tin chi tiết của Item: {itemData.name}");

                        // Cập nhật UI mỗi khi có một Item mới được thêm vào chi tiết
                        onInventoryUpdated?.Invoke();
                        Debug.Log("Đã gọi onInventoryUpdated sau khi thêm thông tin chi tiết.");
                    }
                    else
                    {
                        Debug.LogWarning($"Không tìm thấy thông tin chi tiết cho item: {playerItem.itemId}");
                    }
                });
            }
            else
            {
                // Nếu đã có thông tin chi tiết, cũng gọi onInventoryUpdated để cập nhật UI
                onInventoryUpdated?.Invoke();
                Debug.Log("Đã gọi onInventoryUpdated khi đã có thông tin chi tiết sẵn.");
            }
        }
    }


    // Thêm vật phẩm vào Inventory từ Ground
    public void AddItemToInventory(string itemId)
    {
        var groundItem = groundItems.Find(i => i.itemId == itemId);
        if (groundItem != null)
        {
            var inventoryItem = inventory.Find(i => i.itemId == itemId);
            if (inventoryItem != null)
            {
                inventoryItem.quantity += groundItem.quantity;  // Cộng dồn số lượng vào Inventory nếu đã có sẵn
            }
            else
            {
                inventory.Add(new Inventory(itemId, groundItem.quantity));  // Thêm item mới vào Inventory
            }

            groundItems.Remove(groundItem);  // Xóa khỏi GroundItems
            Debug.Log($"Đã thêm item {itemId} vào Inventory.");
        }
    }

    // Xóa vật phẩm khỏi Inventory và thêm vào Ground
    public void RemoveItemFromInventory(string itemId)
    {
        var inventoryItem = inventory.Find(i => i.itemId == itemId);
        if (inventoryItem != null)
        {
            var groundItem = groundItems.Find(i => i.itemId == itemId);
            if (groundItem != null)
            {
                groundItem.quantity += inventoryItem.quantity;  // Cộng dồn số lượng vào GroundItems nếu đã có sẵn
            }
            else
            {
                groundItems.Add(new Inventory(itemId, inventoryItem.quantity));  // Thêm item mới vào GroundItems
            }

            inventory.Remove(inventoryItem);  // Xóa khỏi Inventory
            Debug.Log($"Đã xóa item {itemId} khỏi Inventory.");
        }
    }
}
