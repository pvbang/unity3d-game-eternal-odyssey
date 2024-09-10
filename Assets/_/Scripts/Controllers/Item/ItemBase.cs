using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemBase : MonoBehaviour
{
    [Header("Save To Database")]
    public bool save = false;
    
    [Header("Item Information")]
    public Item item;

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
            if (item.itemId == "")
            {
                item.itemId = gameObject.name;
            }

            // thêm item vào database
            StartCoroutine(ItemManager.Add(item));
        }
    }
}
