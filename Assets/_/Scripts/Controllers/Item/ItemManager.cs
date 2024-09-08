using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class ItemManager : MonoBehaviour
{
    public bool saveItemToFirebase = false;

    DatabaseReference databaseReference;

    void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Start()
    {
        if (saveItemToFirebase)
        {
            AddItemFromItemBase();
        }
    }

    public void CreateNewItem(Item item)
    {
        string json = JsonUtility.ToJson(item);
        Debug.Log(json);

        string itemId = RandomManager.GenerateRandomID();
        databaseReference.Child("items").Child(itemId).SetRawJsonValueAsync(json);
        //databaseReference.Child("users").Child(itemId).Child("test").SetValueAsync(item.name);

        Debug.Log("Item has been saved successfully!");
    }

    private void AddItemFromItemBase()
    {
        Debug.Log("Attempting to add item from ItemBase.");
        ItemBase itemBase = gameObject.GetComponent<ItemBase>();
        Item item = itemBase.item;

        Debug.Log("Item name: " + item.name);

        CreateNewItem(item);
    }
}
