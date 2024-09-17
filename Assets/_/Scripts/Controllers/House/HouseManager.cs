using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    // thêm house mới vào database
    // cách gọi hàm: StartCoroutine(HouseManager.Add(house));
    public static IEnumerator Add(House house)
    {
        string json = JsonUtility.ToJson(house);

        var task = FirebaseManager.instance.databaseReference.
            Child("houses").
            Child(house.houseId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm house: " + house.houseId);
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm house thành công: " + house.houseId);
        }
    }

    // lấy thông tin house từ id
    // cách gọi hàm: HouseManager.GetHouseById("id", house
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
                Debug.Log("Lỗi lấy thông tin house từ id: " + houseId + " | Exception: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    House house = JsonUtility.FromJson<House>(json);
                    callback(house);
                }
                else
                {
                    Debug.Log("Không tồn tại house với id: " + houseId);
                    callback(null);
                }
            }
        });
    }

    // lấy thông tin tất cả houses
    // cách gọi hàm: HouseManager.GetHouses(houses => { Debug.Log(houses); });
    public static void GetHouses(Action<List<House>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("houses");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin tất cả houses | Exception: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    List<House> houses = new List<House>();

                    foreach (var child in snapshot.Children)
                    {
                        string json = child.GetRawJsonValue();
                        House house = JsonUtility.FromJson<House>(json);
                        houses.Add(house);
                    }

                    callback(houses);
                }
                else
                {
                    Debug.Log("Không tồn tại house nào");
                    callback(null);
                }
            }
        });
    }

    // cập nhật thông tin house
    // cách gọi hàm: StartCoroutine(HouseManager.Update(house));
    public static IEnumerator Update(House house)
    {
        string json = JsonUtility.ToJson(house);

        var task = FirebaseManager.instance.databaseReference
            .Child("houses")
            .Child(house.houseId)
            .SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật thông tin house: " + house.houseId);
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật thông tin house thành công: " + house.houseId);
        }
    }

    // xóa house
    // cách gọi hàm: StartCoroutine(HouseManager.Delete(houseId));
    public static IEnumerator Delete(string houseId)
    {
        var task = FirebaseManager.instance.databaseReference
            .Child("houses")
            .Child(houseId)
            .RemoveValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi xóa house: " + houseId);
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa house thành công: " + houseId);
        }
    }

    // cập nhật một hoặc nhiều trường của house
    // cách gọi hàm: StartCoroutine(HouseManager.UpdateFields("houseId", new Dictionary<string, object> { {"fieldName", fieldValue}, ... }));
    public static IEnumerator UpdateFields(string houseId, Dictionary<string, object> fieldsToUpdate)
    {
        Debug.Log("UpdateFields");
        var task = FirebaseManager.instance.databaseReference
            .Child("houses")
            .Child(houseId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật các trường của house: " + houseId);
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật các trường của house thành công: " + houseId);
        }
    }

   
}
