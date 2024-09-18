using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialsUpdateManager : MonoBehaviour
{
    public List<MaterialsUpdate> materialsUpdate;

    [HideInInspector]
    public List<string> itemIds = new();   // Danh sách itemId từ Firebase

    // Lấy materialsUpdate
    public List<MaterialsUpdate> GetMaterialsUpdate()
    {
        return this.materialsUpdate;
    }


    // lấy itemIds từ MaterialsUpdateManager
    public List<string> GetItemIds()
    {
        return itemIds;
    }
}
