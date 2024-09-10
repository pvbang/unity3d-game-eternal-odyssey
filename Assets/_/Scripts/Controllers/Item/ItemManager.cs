using Firebase.Database;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    // thêm item mới vào database
    // cách gọi hàm: StartCoroutine(ItemManager.Add(item));
    public static IEnumerator Add(Item item)
    {
        ItemData itemData = new(item);
        string json = JsonUtility.ToJson(itemData);

        var task = FirebaseManager.instance.databaseReference.
            Child("items").
            Child(item.itemId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm item");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm item thành công");
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
                Debug.Log("Lỗi lấy thông tin item từ id: " + task.Exception);
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
                    Debug.Log("Không tìm thấy item từ id");
                    callback(null);
                }
            }
        });
    }


    // cập nhật giá trị của item
    // cách gọi hàm: StartCoroutine(ItemManager.Update(item));
    public static IEnumerator Update(Item item)
    {
        string json = JsonUtility.ToJson(item);
        Debug.Log(json);

        var task = FirebaseManager.instance.databaseReference.
            Child("items").
            Child(item.itemId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật item");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật item thành công");
        }
    }


    // cập nhật một số trường của item
    // cách gọi hàm: StartCoroutine(ItemManager.UpdateFields("id", new { name = "new name", price = 100 }));
    public static IEnumerator UpdateFields(string itemId, object fields)
    {
        var task = FirebaseManager.instance.databaseReference.
            Child("items").
            Child(itemId).
            UpdateChildrenAsync((IDictionary<string, object>)fields);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật item");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật item thành công");
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
            Debug.LogError("Lỗi xóa item");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa item thành công");
        }
    }
}
