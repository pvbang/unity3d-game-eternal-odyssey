using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.IO;
using System;
public class WeaponManager : MonoBehaviour
{
    public static IEnumerator AddWeapon(Weapon weapon)
    {
        string json = JsonUtility.ToJson(weapon);

        var task = FirebaseManager.instance.databaseReference.
            Child("weapons").
            Child(weapon.weaponId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm weapon");

        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm weapon thành công");
        }
    }

    // lấy thông tin item từ id
    // cách gọi hàm: ItemManager.GetItemById("id", item => { Debug.Log(item); });
    public static void GetWeaponById(string weaponId, Action<Weapon> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("weapons")
            .Child(weaponId);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin weapon từ id: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    Weapon weapon = JsonUtility.FromJson<Weapon>(snapshot.GetRawJsonValue());
                    callback(weapon);
                }
                else
                {
                    Debug.Log("Không tìm thấy weapon từ id");
                    callback(null);
                }
            }
        });
    }

    // cập nhật giá trị của item
    // cách gọi hàm: StartCoroutine(WeaponManager.UpdateWeapon(item));
    public static IEnumerator UpdateWeapon(Weapon weapon)
    {
        string json = JsonUtility.ToJson(weapon);

        var task = FirebaseManager.instance.databaseReference.
            Child("weapons").
            Child(weapon.weaponId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật weapon");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật weapon thành công");
        }
    }


    // cập nhật một hoặc nhiều trường của weapon
    // cách gọi hàm: StartCoroutine(WeaponManager.UpdateFieldsWeapon("weaponId", new Dictionary<string, object> { {"fieldName", fieldValue}, ... }));
    public static IEnumerator UpdateFieldsWeapon(string weaponId, Dictionary<string, object> fieldsToUpdate)
    {
        var task = FirebaseManager.instance.databaseReference
            .Child("weapons")
            .Child(weaponId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật các trường của weapon");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật các trường của weapon thành công");
        }
    }


    // xóa item
    // cách gọi hàm: StartCoroutine(WeaponManager.DeleteWeapon("id"));
    public static IEnumerator DeleteWeapon(string weaponId)
    {
        var task = FirebaseManager.instance.databaseReference.
            Child("weapons").
            Child(weaponId).
            RemoveValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi xóa weapons");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa weapons thành công");
        }
    }
}