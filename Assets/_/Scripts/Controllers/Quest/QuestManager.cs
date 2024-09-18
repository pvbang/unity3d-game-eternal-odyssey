using Firebase.Database;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public class QuestManager : MonoBehaviour
{
    public List<string> questIds = new();

    // thêm quest mới vào database
    // cách gọi hàm: StartCoroutine(ItemManager.AddQuest(item));
    public static IEnumerator AddQuest(Quest quest)
    {
        string json = JsonUtility.ToJson(quest);

        var task = FirebaseManager.instance.databaseReference.
            Child("quests").
            Child(quest.questId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm quest");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm quest thành công");
        }
    }


    // lấy thông tin quest từ id
    // cách gọi hàm: QuestManager.GetQuestById("id", item => { Debug.Log(item); });
    public static void GetQuestById(string questId, Action<Quest> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("quests")
            .Child(questId);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin quest từ id: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    Quest quest = JsonUtility.FromJson<Quest>(snapshot.GetRawJsonValue());
                    callback(quest);
                }
                else
                {
                    Debug.Log("Không tìm thấy quest từ id");
                    callback(null);
                }
            }
        });
    }


    // cập nhật giá trị của quest
    // cách gọi hàm: StartCoroutine(QuestManager.UpdateQuest(quest));
    public static IEnumerator UpdateQuest(Quest quest)
    {
        string json = JsonUtility.ToJson(quest);

        var task = FirebaseManager.instance.databaseReference.
            Child("quests").
            Child(quest.questId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật quest");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật quest thành công");
        }
    }


    // cập nhật một hoặc nhiều trường của quest
    // cách gọi hàm: StartCoroutine(QuestManager.UpdateFieldsQuest("questId", new Dictionary<string, object> { {"fieldName", fieldValue}, ... }));
    public static IEnumerator UpdateFieldsQuest(string questId, Dictionary<string, object> fieldsToUpdate)
    {
        var task = FirebaseManager.instance.databaseReference
            .Child("quests")
            .Child(questId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật các trường của quest");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật các trường của quest thành công");
        }
    }


    // xóa quest
    // cách gọi hàm: StartCoroutine(QuestManager.DeleteQuest("id"));
    public static IEnumerator DeleteQuest(string questId)
    {
        var task = FirebaseManager.instance.databaseReference.
            Child("quests").
            Child(questId).
            RemoveValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi xóa quest");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa quest thành công");
        }
    }
}

