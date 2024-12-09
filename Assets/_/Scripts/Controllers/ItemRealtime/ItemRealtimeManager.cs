using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class ItemRealtimeManager : MonoBehaviour
{
    public static void SaveChest(ItemRealtime chest, System.Action<bool> callback = null)
    {
        var chestRef = FirebaseManager.instance.databaseReference.Child("item_realtime").Child(chest.chestId);

        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        string json = JsonConvert.SerializeObject(chest, settings);

        chestRef.SetRawJsonValueAsync(json).ContinueWith(task =>
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

    public static void AddItemToChest(string chestId, string itemId, MaterialsUpdate newItem, System.Action<bool> callback = null)
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

    public static void RemoveItemFromChest(string chestId, string itemId, int quantity, System.Action<bool> callback = null)
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
    public static void LoadChest(string chestId, System.Action<ItemRealtime> callback)
    {
        var chestRef = FirebaseManager.instance.databaseReference.Child("item_realtime").Child(chestId);

        chestRef.GetValueAsync().ContinueWith(task =>
        {
            if (!task.IsCompleted || task.Exception != null)
            {
                Debug.LogError($"Lỗi khi lấy dữ liệu rương: {task.Exception?.Message}");
                callback?.Invoke(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string rawData = snapshot.GetRawJsonValue();
                    try
                    {
                        ItemRealtime chest = JsonConvert.DeserializeObject<ItemRealtime>(rawData);
                        Debug.Log($"Rương load từ firebase có {chest.items.Count} items");
                        callback?.Invoke(chest);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Lỗi khi parse dữ liệu rương: {ex.Message}");
                        callback?.Invoke(null);
                    }
                }
                else
                {
                    Debug.Log($"Không tìm thấy rương với ID: {chestId}");
                    callback?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError($"Lỗi khi lấy dữ liệu rương: {task.Exception}");
                callback?.Invoke(null);
            }
        });
    }
    public static void CreateChestAtPosition(string chestId, Vector3 position, string itemId, MaterialsUpdate itemData, System.Action<bool> callback)
    {
        ItemRealtime newChest = new ItemRealtime(chestId, new Location(position.x, position.y, position.z));
        if (!string.IsNullOrEmpty(itemId) && itemData != null)
        {
            newChest.items.Add(itemId, itemData);
        }

        // Lưu rương vào Firebase
        SaveChest(newChest, callback);
    }

    public static void LoadAllChests(System.Action<List<ItemRealtime>> callback)
    {
        var chestsRef = FirebaseManager.instance.databaseReference.Child("item_realtime");

        chestsRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    List<ItemRealtime> allChests = new List<ItemRealtime>();
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        string json = childSnapshot.GetRawJsonValue();
                        ItemRealtime chest = JsonConvert.DeserializeObject<ItemRealtime>(json);
                        allChests.Add(chest);
                    }
                    callback?.Invoke(allChests);
                }
                else
                {
                    Debug.LogWarning("Không có rương nào trong cơ sở dữ liệu.");
                    callback?.Invoke(new List<ItemRealtime>());
                }
            }
            else
            {
                Debug.LogError($"Lỗi khi tải tất cả rương: {task.Exception}");
                callback?.Invoke(null);
            }
        });
    }

    public static void ListenToChestChanges(System.Action<ItemRealtime, string> onChestAdded, System.Action<string> onChestRemoved)
    {
        var chestRef = FirebaseManager.instance.databaseReference.Child("item_realtime");

        chestRef.ChildAdded += (sender, args) =>
        {
            if (args.Snapshot.Exists)
            {
                ItemRealtime newChest = JsonUtility.FromJson<ItemRealtime>(args.Snapshot.GetRawJsonValue());
                onChestAdded?.Invoke(newChest, args.Snapshot.Key);
            }
        };

        chestRef.ChildRemoved += (sender, args) =>
        {
            if (args.Snapshot.Exists)
            {
                string chestId = args.Snapshot.Key;
                onChestRemoved?.Invoke(chestId);
            }
        };
    }
}
