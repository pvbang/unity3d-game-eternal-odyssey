using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestWorldManager : MonoBehaviour
{
    public static void AcceptQuest(string questId, string worldId, Action<bool, string> callback)
    {
        var questRef = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId)
            .Child("questWorld");

        // Lấy danh sách nhiệm vụ từ Firebase
        questRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                callback(false, "Lỗi khi lấy thông tin nhiệm vụ: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Tìm quest với questId
                Debug.Log("snapshot.ChildrenCount: " + snapshot.ChildrenCount);
                foreach (var questSnapshot in snapshot.Children)
                {
                    string currentQuestId = questSnapshot.Child("questId").Value.ToString();
                    Debug.Log("currentQuestId: " + currentQuestId);
                    if (currentQuestId == questId)
                    {
                        int currentStatus = int.Parse(questSnapshot.Child("status").Value.ToString());

                        // Chỉ cho phép nhận nhiệm vụ nếu status là 0 (NotStarted)
                        if (currentStatus == 0)
                        {
                            Dictionary<string, object> updateData = new Dictionary<string, object>
                        {
                            { "status", 1 } // 1 = InProgress
                        };

                            // Cập nhật trạng thái nhiệm vụ
                            questRef.Child(questSnapshot.Key).UpdateChildrenAsync(updateData).ContinueWith(updateTask =>
                            {
                                if (updateTask.IsFaulted)
                                {
                                    callback(false, "Lỗi khi nhận nhiệm vụ: " + updateTask.Exception);
                                }
                                else
                                {
                                    callback(true, "Nhiệm vụ đã được nhận: " + questId);
                                }
                            });
                        }
                        else
                        {
                            callback(false, "Nhiệm vụ đã ở trạng thái InProgress hoặc Complete.");
                        }

                        return;
                    }
                }

                callback(false, "Không tìm thấy nhiệm vụ với ID: " + questId);
            }
        });
    }
    public static void CompleteQuest(string questId, string worldId, Action<bool, string> callback)
    {
        var questRef = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId)
            .Child("questWorld");

        var questDetailRef = FirebaseManager.instance.databaseReference
            .Child("quests")
            .Child(questId);

        // Lấy thông tin questWorld
        questRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                callback(false, "Lỗi khi lấy thông tin nhiệm vụ từ questWorld: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Tìm quest với questId
                foreach (var questSnapshot in snapshot.Children)
                {
                    string currentQuestId = questSnapshot.Child("quest").Child("questId").Value.ToString();

                    if (currentQuestId == questId)
                    {
                        int currentStatus = int.Parse(questSnapshot.Child("status").Value.ToString());

                        // Chỉ xử lý nếu trạng thái là InProgress (1)
                        if (currentStatus == 1)
                        {
                            var progress = questSnapshot.Child("progress_quest_world");

                            // Lấy completionConditions từ quest chi tiết
                            questDetailRef.GetValueAsync().ContinueWith(detailTask =>
                            {
                                if (detailTask.IsFaulted)
                                {
                                    callback(false, "Lỗi khi lấy thông tin chi tiết của quest: " + detailTask.Exception);
                                    return;
                                }

                                if (detailTask.IsCompleted)
                                {
                                    DataSnapshot detailSnapshot = detailTask.Result;

                                    // Lấy danh sách điều kiện hoàn thành
                                    var completionConditions = detailSnapshot.Child("completionConditions");

                                    bool isCompleted = true;

                                    // Kiểm tra từng điều kiện
                                    foreach (var condition in completionConditions.Children)
                                    {
                                        string requiredItemId = condition.Child("itemId").Value.ToString();
                                        int requiredQuantity = int.Parse(condition.Child("quantity").Value.ToString());

                                        // Tìm điều kiện tương ứng trong progress_quest_world
                                        bool conditionMet = false;
                                        foreach (var progressItem in progress.Children)
                                        {
                                            string progressItemId = progressItem.Child("itemId").Value.ToString();
                                            int progressQuantity = int.Parse(progressItem.Child("quantity").Value.ToString());

                                            if (progressItemId == requiredItemId && progressQuantity >= requiredQuantity)
                                            {
                                                conditionMet = true;
                                                break;
                                            }
                                        }

                                        // Nếu bất kỳ điều kiện nào không được đáp ứng, nhiệm vụ không hoàn thành
                                        if (!conditionMet)
                                        {
                                            isCompleted = false;
                                            break;
                                        }
                                    }

                                    if (isCompleted)
                                    {
                                        // Cập nhật trạng thái nhiệm vụ thành Complete (2)
                                        Dictionary<string, object> updateData = new Dictionary<string, object>
                                    {
                                        { "status", 2 } // 2 = Complete
                                    };

                                        questRef.Child(questSnapshot.Key).UpdateChildrenAsync(updateData).ContinueWith(updateTask =>
                                        {
                                            if (updateTask.IsFaulted)
                                            {
                                                callback(false, "Lỗi khi cập nhật trạng thái nhiệm vụ: " + updateTask.Exception);
                                            }
                                            else
                                            {
                                                callback(true, "Nhiệm vụ đã hoàn thành: " + questId);
                                            }
                                        });
                                    }
                                    else
                                    {
                                        callback(false, "Nhiệm vụ chưa được hoàn thành (chưa đáp ứng đủ điều kiện).");
                                    }
                                }
                            });
                        }
                        else
                        {
                            callback(false, "Nhiệm vụ chưa được nhận hoặc đã hoàn thành.");
                        }

                        return;
                    }
                }

                callback(false, "Không tìm thấy nhiệm vụ với ID: " + questId);
            }
        });
    }

}
