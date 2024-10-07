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
    void Start()
    {
        // Lấy worldId và userId từ GameManager
        string worldId = FirebaseManager.instance.currentWorldId;
        string userId = FirebaseManager.instance.currentUserId;

        if (!string.IsNullOrEmpty(worldId) && !string.IsNullOrEmpty(userId))
        {
            LoadInventoryDetailsFromFirebase(worldId, userId);
        }
        else
        {
            Debug.LogError("World ID hoặc User ID không hợp lệ.");
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
                    }
                });
            }
        }
    }
}
