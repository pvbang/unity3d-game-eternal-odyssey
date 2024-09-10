using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public bool saveItemToFirebase = false;

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
        FirebaseManager.instance.databaseReference.Child("items").Child(itemId).SetRawJsonValueAsync(json);

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
