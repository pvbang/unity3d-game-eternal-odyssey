using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    // thêm house mới vào database
    // cách gọi hàm: StartCoroutine(HouseManager.AddHouse(house));
    public static IEnumerator AddHouse(House house)
    {
        string json = JsonUtility.ToJson(house);

        var task = FirebaseManager.instance.databaseReference.
            Child("houses").
            Child(house.houseId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm house");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm house thành công");
        }
    }


    // lấy thông tin house từ id
    // cách gọi hàm: HouseManager.GetHouseById("id", house => { Debug.Log(house); });
    public static void GetHouseById(string houseId, Action<House> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("houses")
            .Child(houseId);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin house từ id: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    House house = JsonUtility.FromJson<House>(snapshot.GetRawJsonValue());
                    callback(house);
                }
                else
                {
                    Debug.Log("Không tìm thấy house từ id");
                    callback(null);
                }
            }
        });
    }


    // cập nhật giá trị của house
    // cách gọi hàm: StartCoroutine(HouseManager.UpdateHouse(house));
    public static IEnumerator UpdateHouse(House house)
    {
        string json = JsonUtility.ToJson(house);

        var task = FirebaseManager.instance.databaseReference.
            Child("houses").
            Child(house.houseId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật house");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật house thành công");
        }
    }


    // cập nhật một hoặc nhiều trường của house
    // cách gọi hàm: StartCoroutine(HouseManager.UpdateFieldsHouse("houseId", new Dictionary<string, object> { {"fieldName", fieldValue}, ... }));
    public static IEnumerator UpdateFieldsHouse(string houseId, Dictionary<string, object> fieldsToUpdate)
    {
        Debug.Log("UpdateFieldsHouse");
        var task = FirebaseManager.instance.databaseReference
            .Child("houses")
            .Child(houseId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật các trường của house");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật các trường của house thành công");
        }
    }


    // xóa house
    // cách gọi hàm: StartCoroutine(HouseManager.DeleteHouse("id"));
    public static IEnumerator DeleteHouse(string houseId)
    {
        var task = FirebaseManager.instance.databaseReference.
            Child("houses").
            Child(houseId).
            RemoveValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi xóa house");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa house thành công");
        }
    }
}
