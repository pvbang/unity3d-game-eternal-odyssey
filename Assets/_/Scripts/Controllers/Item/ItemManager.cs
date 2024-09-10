using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    // thêm item mới vào database
    // cách gọi hàm: StartCoroutine(ItemManager.Add(item));
    public static IEnumerator Add(Item item)
    {
        string type = item.TypeString;
        string properties = item.PropertiesString;

        string json = JsonUtility.ToJson(item);
        Debug.Log(json);

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
}
