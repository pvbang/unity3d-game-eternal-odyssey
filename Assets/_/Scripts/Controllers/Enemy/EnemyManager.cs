using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // thêm enemy mới vào database
    // cách gọi hàm: StartCoroutine(EnemyManager.AddEnemy(enemy));
    public static IEnumerator AddEnemy(Enemy enemy)
    {
        string json = JsonUtility.ToJson(enemy);

        var task = FirebaseManager.instance.databaseReference.
            Child("enemys").
            Child(enemy.enemyId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm enemy");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm enemy thành công");
        }
    }


    // lấy thông tin enemy từ id
    // cách gọi hàm: EnemyManager.GetEnemyById("id", enemy => { Debug.Log(enemy); });
    public static void GetEnemyById(string enemyId, Action<Enemy> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("enemys")
            .Child(enemyId);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin enemy từ id: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    Enemy enemy = JsonUtility.FromJson<Enemy>(snapshot.GetRawJsonValue());
                    callback(enemy);
                }
                else
                {
                    Debug.Log("Không tìm thấy enemy từ id");
                    callback(null);
                }
            }
        });
    }


    // cập nhật giá trị của item
    // cách gọi hàm: StartCoroutine(EnemyManager.UpdateEnemy(enemy));
    public static IEnumerator UpdateEnemy(Enemy enemy)
    {
        string json = JsonUtility.ToJson(enemy);

        var task = FirebaseManager.instance.databaseReference.
            Child("enemys").
            Child(enemy.enemyId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật enemy");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật enemy thành công");
        }
    }


    // cập nhật một hoặc nhiều trường của enemy
    // cách gọi hàm: StartCoroutine(EnemyManager.UpdateFieldsEnemy("enemyId", new Dictionary<string, object> { {"fieldName", fieldValue}, ... }));
    public static IEnumerator UpdateFieldsEnemy(string enemyId, Dictionary<string, object> fieldsToUpdate)
    {
        Debug.Log("UpdateFieldsEnemy");
        var task = FirebaseManager.instance.databaseReference
            .Child("enemys")
            .Child(enemyId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật các trường của enemy");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật các trường của enemy thành công");
        }
    }


    // xóa enemy
    // cách gọi hàm: StartCoroutine(EnemyManager.DeleteEnemy("id"));
    public static IEnumerator DeleteEnemy(string enemyId)
    {
        var task = FirebaseManager.instance.databaseReference.
            Child("enemys").
            Child(enemyId).
            RemoveValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi xóa enemy");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa enemy thành công");
        }
    }
}
