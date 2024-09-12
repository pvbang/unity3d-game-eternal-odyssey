using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemBase : MonoBehaviour
{
    [Header("Save To Database")]
    public bool save = false;

    [Header("Item Information")]
    public Item item;

    [HideInInspector]
    public List<string> itemIds = new();   // Danh sách itemId từ Firebase

    private void Start()
    {
        if (item == null)
        {
            return;
        }

        // nếu saveToDatabase = true thì lưu item này vào database
        if (save)
        {
            // nếu item này chưa nhập vào id thì sẽ lấy id của gameObject, không thì sẽ lấy id mình nhập vào của item
            if (string.IsNullOrEmpty(item.itemId))
            {
                item.itemId = gameObject.name;
            }

            // thêm item vào database
            StartCoroutine(ItemManager.Add(item));
        }
    }
}
