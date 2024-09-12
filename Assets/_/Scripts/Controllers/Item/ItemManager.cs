using Firebase.Database;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Firebase.Extensions;

public class ItemManager : MonoBehaviour
{
    public List<string> itemIds = new();

    // thêm item mới vào database
    // cách gọi hàm: StartCoroutine(ItemManager.Add(item));
    public static IEnumerator Add(Item item)
    {
        string json = JsonUtility.ToJson(item);

        var task = FirebaseManager.instance.databaseReference.
            Child("items").
            Child(item.itemId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm item: " + item.itemId);
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm item thành công: " + item.itemId);
        }
    }


    // lấy thông tin item từ id
    // cách gọi hàm: ItemManager.GetItemById("id", item => { Debug.Log(item); });
    public static void GetItemById(string itemId, Action<Item> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("items")
            .Child(itemId);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin item từ id: " + itemId + " | Exception: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    Item item = JsonUtility.FromJson<Item>(snapshot.GetRawJsonValue());
                    callback(item);
                }
                else
                {
                    Debug.Log("Không tìm thấy item từ id: " + itemId);
                    callback(null);
                }
            }
        });
    }

    // lấy thông tin tất cả items
    // cách gọi hàm: ItemManager.GetItems(items => { Debug.Log(items); });
    public static void GetItems(Action<List<Item>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference.Child("items");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin tất cả items: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<Item> items = new List<Item>();

                foreach (DataSnapshot itemSnapshot in snapshot.Children)
                {
                    Item item = JsonUtility.FromJson<Item>(itemSnapshot.GetRawJsonValue());
                    items.Add(item);
                }

                callback(items);
            }
        });
    }


    // cập nhật giá trị của item
    // cách gọi hàm: StartCoroutine(ItemManager.Update(item));
    public static IEnumerator Update(Item item)
    {
        string json = JsonUtility.ToJson(item);

        var task = FirebaseManager.instance.databaseReference.
            Child("items").
            Child(item.itemId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật item: " + item.itemId);
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật item thành công: " + item.itemId);
        }
    }


    // cập nhật một hoặc nhiều trường của item
    // cách gọi hàm: StartCoroutine(ItemManager.UpdateFields("itemId", new Dictionary<string, object> { {"fieldName", fieldValue}, ... }));
    public static IEnumerator UpdateFields(string itemId, Dictionary<string, object> fieldsToUpdate)
    {
        Debug.Log("UpdateFields");
        var task = FirebaseManager.instance.databaseReference
            .Child("items")
            .Child(itemId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật các trường của item: " + itemId);
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật các trường của item thành công: " + itemId);
        }
    }


    // xóa item
    // cách gọi hàm: StartCoroutine(ItemManager.Delete("id"));
    public static IEnumerator Delete(string itemId)
    {
        var task = FirebaseManager.instance.databaseReference.
            Child("items").
            Child(itemId).
            RemoveValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi xóa item" + itemId);
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa item thành công: " + itemId);
        }
    }

    // lấy danh sách itemIds từ Firebase
    public void GetListItemId()
    {
        DatabaseReference databaseReference;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        databaseReference.Child("items").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                itemIds.Clear(); // Xóa danh sách cũ để làm mới

                foreach (DataSnapshot itemSnapshot in snapshot.Children)
                {
                    string itemId = itemSnapshot.Child("itemId").Value.ToString(); 
                    itemIds.Add(itemId);
                }

                //Debug.Log("Item IDs loaded from Firebase.");
            }
            else
            {
                Debug.LogError("Failed to load items from Firebase: " + task.Exception);
            }
        });
    }
}
