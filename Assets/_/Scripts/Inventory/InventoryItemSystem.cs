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

    public bool isDataLoaded = false;  // Biến để kiểm tra xem dữ liệu đã được tải từ Firebase chưa

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

    private void Start()
    {
        // Tải thông tin chi tiết của item từ Firebase
        LoadItemDetails(() =>
        {
            Debug.Log("Đã tải xong thông tin item từ Firebase.");

            // Sau khi đã có thông tin chi tiết, tải danh sách Inventory từ Firebase
            LoadInventoryFromFirebase(inventoryList =>
            {
                this.inventory = inventoryList;  // Gán danh sách inventory
                Debug.Log("Đã tải xong danh sách inventory từ Firebase.");
                onInventoryUpdated?.Invoke();  // Gọi hàm để cập nhật UI
            });
        });
    }

    // Hàm lấy thông tin chi tiết của Inventory từ Firebase
    public static void LoadInventoryFromFirebase(Action<List<Inventory>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child("GY99YQGNYFI8KTIMPEELYTDPY8IO3Q")
            .Child("playerWorld")
            .Child("1")
            .Child("inventorys");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy danh sách inventory: " + task.Exception);
                callback(null);  // Trả về null trong trường hợp lỗi
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                List<Inventory> inventoryList = new List<Inventory>();

                foreach (var child in snapshot.Children)
                {
                    Inventory tempInventory = JsonUtility.FromJson<Inventory>(child.GetRawJsonValue());

                    // Kiểm tra nếu itemId tồn tại trong itemDetails
                    if (instance.itemDetails.TryGetValue(tempInventory.itemId, out Item itemDetail))
                    {
                        // Tạo Inventory mới với đối tượng Item đầy đủ và số lượng
                        Inventory completeInventory = new Inventory(itemDetail, tempInventory.quantity);
                        inventoryList.Add(completeInventory);
                    }
                    else
                    {
                        Debug.LogWarning($"Không tìm thấy thông tin chi tiết cho item với ID: {tempInventory.itemId}");
                    }
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
    public void AddItemToInventory(string itemId)
    {
        var groundItem = groundItems.Find(i => i.item.itemId == itemId);
        if (groundItem != null)
        {
            var inventoryItem = inventory.Find(i => i.item.itemId == itemId);
            if (inventoryItem != null)
            {
                inventoryItem.quantity += groundItem.quantity;  // Cộng dồn số lượng vào Inventory nếu đã có sẵn
            }
            else
            {
                inventory.Add(new Inventory(groundItem.item, groundItem.quantity));  // Thêm item mới vào Inventory với thông tin đầy đủ
            }

            groundItems.Remove(groundItem);  // Xóa khỏi GroundItems
            Debug.Log($"Đã thêm item {itemId} vào Inventory.");
        }
    }

    // Xóa vật phẩm khỏi Inventory và thêm vào Ground
    public void RemoveItemFromInventory(string itemId)
    {
        var inventoryItem = inventory.Find(i => i.item.itemId == itemId);
        if (inventoryItem != null)
        {
            var groundItem = groundItems.Find(i => i.item.itemId == itemId);
            if (groundItem != null)
            {
                groundItem.quantity += inventoryItem.quantity;  // Cộng dồn số lượng vào GroundItems nếu đã có sẵn
            }
            else
            {
                groundItems.Add(new Inventory(inventoryItem.item, inventoryItem.quantity));  // Thêm item mới vào GroundItems
            }

            inventory.Remove(inventoryItem);  // Xóa khỏi Inventory
            Debug.Log($"Đã xóa item {itemId} khỏi Inventory.");
        }
    }
}
