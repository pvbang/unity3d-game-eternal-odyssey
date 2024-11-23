using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRealtimeManager : MonoBehaviour
{
    public void SaveChest(ItemRealtime chest, System.Action<bool> callback = null)
    {
        // Tham chiếu đến node `item_realtime` trên Firebase
        var chestRef = FirebaseManager.instance.databaseReference.Child("item_realtime").Child(chest.chestId);

        // Lưu rương vào Firebase
        chestRef.SetRawJsonValueAsync(JsonUtility.ToJson(chest)).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"Rương {chest.chestId} đã được lưu thành công!");
                callback?.Invoke(true);
            }
            else
            {
                Debug.LogError($"Lỗi khi lưu rương {chest.chestId}: {task.Exception}");
                callback?.Invoke(false);
            }
        });
    }

    public void AddItemToChest(string chestId, string itemId, MaterialsUpdate newItem, System.Action<bool> callback = null)
    {
        var itemRef = FirebaseManager.instance.databaseReference
            .Child("item_realtime")
            .Child(chestId)
            .Child("items")
            .Child(itemId);

        itemRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    // Nếu item đã tồn tại, tăng số lượng
                    MaterialsUpdate existingItem = JsonUtility.FromJson<MaterialsUpdate>(snapshot.GetRawJsonValue());
                    existingItem.quantity += newItem.quantity;

                    itemRef.SetRawJsonValueAsync(JsonUtility.ToJson(existingItem)).ContinueWith(innerTask =>
                    {
                        if (innerTask.IsCompleted)
                        {
                            Debug.Log($"Đã thêm {newItem.quantity} vào item {itemId} trong rương {chestId}");
                            callback?.Invoke(true);
                        }
                        else
                        {
                            Debug.LogError($"Lỗi khi tăng số lượng item {itemId}: {innerTask.Exception}");
                            callback?.Invoke(false);
                        }
                    });
                }
                else
                {
                    // Nếu item chưa tồn tại, thêm mới
                    itemRef.SetRawJsonValueAsync(JsonUtility.ToJson(newItem)).ContinueWith(innerTask =>
                    {
                        if (innerTask.IsCompleted)
                        {
                            Debug.Log($"Item mới {itemId} đã được thêm vào rương {chestId}");
                            callback?.Invoke(true);
                        }
                        else
                        {
                            Debug.LogError($"Lỗi khi thêm item mới {itemId}: {innerTask.Exception}");
                            callback?.Invoke(false);
                        }
                    });
                }
            }
            else
            {
                Debug.LogError($"Lỗi khi kiểm tra item {itemId}: {task.Exception}");
                callback?.Invoke(false);
            }
        });
    }

    public void RemoveItemFromChest(string chestId, string itemId, int quantity, System.Action<bool> callback = null)
    {
        var itemRef = FirebaseManager.instance.databaseReference
            .Child("item_realtime")
            .Child(chestId)
            .Child("items")
            .Child(itemId);

        itemRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    MaterialsUpdate existingItem = JsonUtility.FromJson<MaterialsUpdate>(snapshot.GetRawJsonValue());

                    if (existingItem.quantity > quantity)
                    {
                        // Giảm số lượng item
                        existingItem.quantity -= quantity;

                        itemRef.SetRawJsonValueAsync(JsonUtility.ToJson(existingItem)).ContinueWith(innerTask =>
                        {
                            if (innerTask.IsCompleted)
                            {
                                Debug.Log($"Đã giảm {quantity} khỏi item {itemId} trong rương {chestId}");
                                callback?.Invoke(true);
                            }
                            else
                            {
                                Debug.LogError($"Lỗi khi giảm số lượng item {itemId}: {innerTask.Exception}");
                                callback?.Invoke(false);
                            }
                        });
                    }
                    else
                    {
                        // Xóa item nếu số lượng <= 0
                        itemRef.RemoveValueAsync().ContinueWith(innerTask =>
                        {
                            if (innerTask.IsCompleted)
                            {
                                Debug.Log($"Item {itemId} đã bị xóa khỏi rương {chestId}");
                                callback?.Invoke(true);
                            }
                            else
                            {
                                Debug.LogError($"Lỗi khi xóa item {itemId}: {innerTask.Exception}");
                                callback?.Invoke(false);
                            }
                        });
                    }
                }
                else
                {
                    Debug.LogWarning($"Item {itemId} không tồn tại trong rương {chestId}");
                    callback?.Invoke(false);
                }
            }
            else
            {
                Debug.LogError($"Lỗi khi kiểm tra item {itemId}: {task.Exception}");
                callback?.Invoke(false);
            }
        });
    }


}
