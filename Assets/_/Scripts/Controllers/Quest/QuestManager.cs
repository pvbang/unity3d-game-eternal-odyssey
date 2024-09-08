using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.IO; 
public class QuestManager : MonoBehaviour
{
    public bool saveQuestToFirebase = false;
    private DatabaseReference databaseReference;

    void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    void Start()
    {
        if (saveQuestToFirebase)
        {
            AddItemFromItemBase();
        }
    }

    public void CreateNewItem(Quest quest)
    {
        string json = JsonUtility.ToJson(quest);
        string questId = gameObject.name;
        databaseReference.Child("quests").Child(questId).SetRawJsonValueAsync(json);
        Debug.Log("Item has been saved successfully!");
    }

    private void AddItemFromItemBase()
    {
        Debug.Log("Attempting to add item from ItemBase.");
        QuestList questList = gameObject.GetComponent<QuestList>();
        Quest quest = questList.quests;
        CreateNewItem(quest);
    }

}

