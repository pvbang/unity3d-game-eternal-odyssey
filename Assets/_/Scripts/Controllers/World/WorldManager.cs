using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // thêm world mới vào database
    // cách gọi hàm: StartCoroutine(WorldManager.AddWorld(world));
    public static IEnumerator AddWorld(World world)
    {
        string json = JsonUtility.ToJson(world);

        var task = FirebaseManager.instance.databaseReference.
            Child("worlds").
            Child(world.worldId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm world");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm world thành công");
        }
    }


    // thêm world mới


    // lấy thông tin world từ id
    // cách gọi hàm: WorldManager.GetworldById("id", world => { Debug.Log(world); });
    public static void GetWorldById(string worldId, Action<World> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin world từ id: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    World world = JsonUtility.FromJson<World>(snapshot.GetRawJsonValue());
                    callback(world);
                }
                else
                {
                    Debug.Log("Không tìm thấy world từ id");
                    callback(null);
                }
            }
        });
    }


    // lấy danh sách tất cả world
    // cách gọi hàm: WorldManager.GetAllWorlds(worlds => { Debug.Log(worlds); });
    public static void GetAllWorlds(Action<List<World>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("worlds");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy danh sách tất cả world: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                List<World> worlds = new List<World>();

                foreach (var child in snapshot.Children)
                {
                    World world = JsonUtility.FromJson<World>(child.GetRawJsonValue());
                    worlds.Add(world);
                }

                callback(worlds);
            }
        });
    }


    // cập nhật giá trị của world
    // cách gọi hàm: StartCoroutine(WorldManager.UpdateWorld(world));
    public static IEnumerator UpdateWorld(World world)
    {
        string json = JsonUtility.ToJson(world);

        var task = FirebaseManager.instance.databaseReference.
            Child("worlds").
            Child(world.worldId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật world");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật world thành công");
        }
    }


    // cập nhật một hoặc nhiều trường của world
    // cách gọi hàm: StartCoroutine(WorldManager.UpdateFieldsWorld("worldId", new Dictionary<string, object> { {"fieldName", fieldValue}, ... }));
    public static IEnumerator UpdateFieldsWorld(string worldId, Dictionary<string, object> fieldsToUpdate)
    {
        Debug.Log("UpdateFields");
        var task = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật các trường của world");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật các trường của world thành công");
        }
    }


    // xóa world
    // cách gọi hàm: StartCoroutine(WorldManager.DeleteWorld("id"));
    public static IEnumerator DeleteWorld(string worldId)
    {
        var task = FirebaseManager.instance.databaseReference.
            Child("worlds").
            Child(worldId).
            RemoveValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi xóa world");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa world thành công");
        }
    }


}
