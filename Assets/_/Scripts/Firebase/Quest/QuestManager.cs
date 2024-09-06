using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.IO; 
public class QuestManager : MonoBehaviour
{
    private int questCounter = 0;
    private DatabaseReference databaseReference;

    void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    void Start()
    {
      LoadQuestsFromJsonAndSaveToFirebase();
    }

    public void saveQuest(string title, string description, List<string> reward, string requirements, string type)
    {   Quest quest = new Quest(title, description, reward, requirements, type);
        string json = JsonUtility.ToJson(quest);
        string questId = GetFormattedQuestId();
        databaseReference.Child("quests").Child(questId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => 
        {
            if (task.IsCompleted)
            {
                Debug.Log("Quest has been saved successfully!");
            }
            else
            {
                Debug.LogError("Failed to save quest: " + task.Exception);
            }
        });
    }

    private string GetFormattedQuestId()
    {
        questCounter++;
        return "quest_" + questCounter.ToString("D2"); // Định dạng số thành 2 chữ số
    }

     private void LoadQuestsFromJsonAndSaveToFirebase()
    {
        // Tải file JSON từ thư mục Resources
        Debug.Log("Attempting to load quests.json from Resources folder.");
        TextAsset jsonFile = Resources.Load<TextAsset>("quests");

        if (jsonFile == null)
        {
            Debug.LogError("Không tìm thấy file JSON trong thư mục Resources.");
            return;
        }

        Debug.Log("Successfully loaded quests.json from Resources folder.");

        string json = jsonFile.text;
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("File JSON trống hoặc không đọc được.");
            return;
        }

        Debug.Log("JSON content loaded: " + json);

        QuestList questList = JsonUtility.FromJson<QuestList>("{\"quests\":" + json + "}");
        if (questList == null || questList.quests == null)
        {
            Debug.LogError("Không thể chuyển đổi JSON sang đối tượng QuestList.");
            return;
        }

        Debug.Log("Successfully parsed JSON into QuestList object.");

        foreach (Quest quest in questList.quests)
        {
            Debug.Log("Saving quest: " + quest.title);
            saveQuest(quest.title, quest.description, quest.reward, quest.requirements, quest.type);
        }
    }
}

