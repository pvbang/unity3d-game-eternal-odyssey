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
}
