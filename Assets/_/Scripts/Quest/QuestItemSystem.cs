using Firebase.Database;
using NullSave.TOCK.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestItemSystem : MonoBehaviour
{
    public static List<QuestWorld> questsList = new List<QuestWorld>();
    public static QuestItemSystem instance;  // Singleton cho InventoryItemSystem
    public Dictionary<string, Quest> questDetails = new Dictionary<string, Quest>();
    public string worldId;
    public Action onQuestUpdated;

    public void Awake()
    {
        worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadQuestFromFirebase(Action<List<QuestWorld>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId)
            .Child("questWorld");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy danh sách quest: " + task.Exception);
                callback(null);  // Trả về null trong trường hợp lỗi
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<QuestWorld> questList = new List<QuestWorld>();

                Debug.Log($"Tổng số quest: {snapshot.ChildrenCount}");

                foreach (var child in snapshot.Children)
                {
                    QuestWorld questWorld = JsonUtility.FromJson<QuestWorld>(child.GetRawJsonValue());
                    questList.Add(questWorld);
                }

                if (questList == null || questList.Count == 0)
                {
                    Debug.LogWarning("Không tìm thấy quest hoặc quest trống.");
                }
                callback(questList);
            }
        });
    }

    public void LoadQuestDetails(Action callback)
    {
        QuestManager.FetchListQuests((quests, questId) =>
        {
            if (quests == null)
            {
                Debug.LogError("Không thể tải thông tin quests từ Firebase.");
                return;
            }

            // Lưu các item vào itemDetails dictionary
            foreach (var quest in quests)
            {
                if (!questDetails.ContainsKey(quest.questId))
                {
                    questDetails.Add(quest.questId, quest);
                }
            }
            callback?.Invoke();
        });
    }


    public IEnumerator CompareQuestData()
    {
        bool checkCallback = false;

        instance.LoadQuestDetails(() =>
        {
            instance.LoadQuestFromFirebase((questWorlds) =>
            {
                // Tạo danh sách tạm để lưu các mục đã hoàn thiện
                List<QuestWorld> completedQuestList = new List<QuestWorld>();

                foreach (var questItem in questWorlds)
                {
                    // Kiểm tra xem itemDetails có chứa itemId này không
                    if (instance.questDetails.TryGetValue(questItem.questId, out Quest questDetail))
                    {
                        QuestWorld completeInventory = new QuestWorld(questDetail, questItem.status, questItem.progress_quest_world);
                        Debug.Log($"Đã tìm thấy thông tin chi tiết cho item với ID: {questItem.questId}");
                        completedQuestList.Add(completeInventory);
                    }
                    else
                    {
                        Debug.LogWarning($"Không tìm thấy thông tin chi tiết cho item với ID: {questItem.questId}");
                    }
                }

                // **Sắp xếp danh sách hoàn thiện theo questId**
                completedQuestList = completedQuestList.OrderBy(q => q.quest.questId).ToList();

                // Gán danh sách hoàn thiện vào inventory
                Debug.Log("Đã cập nhật danh sách quest full: " + completedQuestList);
                questsList = completedQuestList;
                onQuestUpdated?.Invoke();

                checkCallback = true;
            });
        });

        yield return new WaitUntil(() => checkCallback);
    }

    public QuestWorld GetQuestById(string questId)
    {
        // Lặp qua danh sách questWorld và tìm quest với questId phù hợp
        foreach (var quest in questsList)
        {
            if (quest.quest.questId == questId)
            {
                return quest;
            }
        }
        return null;
    }

}

