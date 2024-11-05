using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public static void DeleteItem(string itemId, int quantity, string worldId, string userId, Action<bool, string> callback)
    {
        FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId)
            .Child("playerWorld")
            .Child(userId)
            .Child("inventorys")
            .GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    callback(false, "Lỗi khi lấy thông tin inventory: " + task.Exception);
                    return;
                }

                if (!task.Result.Exists)
                {
                    callback(false, "Không tìm thấy inventory.");
                    return;
                }

                bool itemDeleted = false;

                foreach (var item in task.Result.Children)
                {
                    string currentItemId = item.Key;
                    var itemData = item.Child("itemId").Value.ToString();

                    if (itemData == itemId)
                    {
                        int currentQuantity = int.Parse(item.Child("quantity").Value.ToString());

                        if (currentQuantity == quantity)
                        {
                            FirebaseManager.instance.databaseReference
                                .Child("worlds")
                                .Child(worldId)
                                .Child("playerWorld")
                                .Child(userId)
                                .Child("inventorys")
                                .Child(currentItemId)
                                .RemoveValueAsync().ContinueWith(deleteTask =>
                                {
                                    if (deleteTask.IsFaulted)
                                    {
                                        callback(false, "Lỗi khi xóa item: " + currentItemId);
                                    }
                                    else
                                    {
                                        callback(true, "Xóa item thành công: " + currentItemId);
                                    }
                                });

                            itemDeleted = true;
                            break;
                        }
                    }
                }

                if (!itemDeleted)
                {
                    callback(false, "Không tìm thấy item hoặc số lượng không khớp.");
                }
            });
    }


    public static void AddItem(string itemId, int quantity, string worldId, string userId, Action<bool, string> callback)
    {
        var inventoryRef = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId)
            .Child("playerWorld")
            .Child(userId)
            .Child("inventorys");

        inventoryRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                callback(false, "Lỗi khi lấy thông tin inventory: " + task.Exception);
                return;
            }

            string newItemKey = inventoryRef.Push().Key;
            var newItem = new Dictionary<string, object>
            {
                { "itemId", itemId },
                { "quantity", quantity }
            };

            inventoryRef.Child(newItemKey).SetValueAsync(newItem).ContinueWith(addItemTask =>
            {
                if (addItemTask.IsFaulted)
                {
                    callback(false, "Lỗi khi thêm item mới: " + addItemTask.Exception);
                }
                else
                {
                    callback(true, "Thêm item mới thành công: " + itemId);
                }
            });
        });
    }

    // Hàm cập nhật số lượng item với callback
    public static void UpdateItemQuantity(string itemId, int newQuantity, string worldId, string userId, Action<bool> callback)
    {
        var inventoryRef = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId)
            .Child("playerWorld")
            .Child(userId)
            .Child("inventorys");

        // Lấy dữ liệu inventory từ Firebase
        inventoryRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Lỗi khi lấy thông tin inventory: " + task.Exception);
                callback(false);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                bool itemUpdated = false;

                foreach (var itemSnapshot in snapshot.Children)
                {
                    string currentItemId = itemSnapshot.Child("itemId").Value.ToString();

                    if (currentItemId == itemId)
                    {
                        if (newQuantity > 0)
                        {
                            // Cập nhật số lượng nếu số lượng mới lớn hơn 0
                            Dictionary<string, object> updateData = new Dictionary<string, object>
                            {
                                { "quantity", newQuantity }
                            };

                            inventoryRef.Child(itemSnapshot.Key).UpdateChildrenAsync(updateData).ContinueWith(updateTask =>
                            {
                                if (updateTask.IsFaulted)
                                {
                                    Debug.LogError("Lỗi khi cập nhật số lượng item: " + updateTask.Exception);
                                    callback(false);
                                }
                                else
                                {
                                    Debug.Log("Cập nhật số lượng item thành công: " + itemId);
                                    callback(true);
                                }
                            });
                        }
                        else
                        {
                            // Xóa item nếu số lượng mới <= 0
                            inventoryRef.Child(itemSnapshot.Key).RemoveValueAsync().ContinueWith(removeTask =>
                            {
                                if (removeTask.IsFaulted)
                                {
                                    Debug.LogError("Lỗi khi xóa item: " + removeTask.Exception);
                                    callback(false);
                                }
                                else
                                {
                                    Debug.Log("Xóa item thành công: " + itemId);
                                    callback(true);
                                }
                            });
                        }

                        itemUpdated = true;
                        break;
                    }
                }

                if (!itemUpdated)
                {
                    Debug.LogWarning("Không tìm thấy item với ID: " + itemId + " trong inventory.");
                    callback(false);
                }
            }
        });
    }
}
