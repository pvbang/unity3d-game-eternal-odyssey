using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager_ : MonoBehaviour
{
    // thêm player mới vào database
    // cách gọi hàm: StartCoroutine(PlayerManager.Add(player));
    public static IEnumerator Add(Player player)
    {
        string json = JsonUtility.ToJson(player);

        var task = FirebaseManager.instance.databaseReference.
            Child("players").
            Child(player.playerId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm player");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm player thành công");
        }
    }

    // lấy thông tin player từ id
    // cách gọi hàm: PlayerManager.GetPlayerById("id", player => { Debug.Log(player); });
    public static void GetPlayerById(string playerId, Action<Player> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("players")
            .Child(playerId);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin player từ id: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    Player player = JsonUtility.FromJson<Player>(snapshot.GetRawJsonValue());
                    callback(player);
                }
                else
                {
                    Debug.Log("Không tìm thấy player từ id");
                    callback(null);
                }
            }
        });
    }

    // cập nhật giá trị của item
    // cách gọi hàm: StartCoroutine(PlayerManager.UpdatePlayer(player));
    public static IEnumerator UpdatePlayer(Player player)
    {
        string json = JsonUtility.ToJson(player);

        var task = FirebaseManager.instance.databaseReference.
            Child("players").
            Child(player.playerId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật player");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật player thành công");
        }
    }

    // cập nhật một hoặc nhiều trường của player
    // cách gọi hàm: StartCoroutine(PlayerManager.UpdateFieldsPlayer("playerId", new Dictionary<string, object> { {"fieldName", fieldValue}, ... }));
    public static IEnumerator UpdateFieldsPlayer(string playerId, Dictionary<string, object> fieldsToUpdate)
    {
        Debug.Log("UpdateFieldsPlayer");
        var task = FirebaseManager.instance.databaseReference
            .Child("players")
            .Child(playerId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật các trường của player");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật các trường của player thành công");
        }
    }

    // xóa player
    // cách gọi hàm: StartCoroutine(PlayerManager.DeletePlayer("id"));
    public static IEnumerator DeletePlayer(string playerId)
    {
        var task = FirebaseManager.instance.databaseReference.
            Child("players").
            Child(playerId).
            RemoveValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi xóa player");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa player thành công");
        }
    }

    // lấy danh sách player
    // cách gọi hàm: PlayerManager.GetPlayers(players => { Debug.Log(players); });
    public static void GetPlayers(Action<List<Player>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("players");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy danh sách player: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    List<Player> players = new List<Player>();

                    foreach (var child in snapshot.Children)
                    {
                        Player player = JsonUtility.FromJson<Player>(child.GetRawJsonValue());
                        players.Add(player);
                    }

                    callback(players);
                }
                else
                {
                    Debug.Log("Không tìm thấy danh sách player");
                    callback(null);
                }
            }
        });
    }

    // lấy danh sách player theo điều kiện
    // cách gọi hàm: PlayerManager.GetPlayersByCondition("fieldName", "value", players => { Debug.Log(players); });
    public static void GetPlayersByCondition(string fieldName, string value, Action<List<Player>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("players")
            .OrderByChild(fieldName)
            .EqualTo(value);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy danh sách player theo điều kiện: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    List<Player> players = new List<Player>();

                    foreach (var child in snapshot.Children)
                    {
                        Player player = JsonUtility.FromJson<Player>(child.GetRawJsonValue());
                        players.Add(player);
                    }

                    callback(players);
                }
                else
                {
                    Debug.Log("Không tìm thấy danh sách player theo điều kiện");
                    callback(null);
                }
            }
        });
    }



}
