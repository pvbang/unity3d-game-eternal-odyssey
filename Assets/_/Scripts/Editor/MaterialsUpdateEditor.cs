using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialsUpdateBase))]
public class MaterialsUpdateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MaterialsUpdateBase materialsUpdateBase = (MaterialsUpdateBase)target;

        GUILayout.Space(10); // Tạo khoảng cách giữa phần Inspector mặc định và nút bấm

        // Nút để tải danh sách itemIds từ Firebase
        if (GUILayout.Button("Load Item IDs from Firebase"))
        {
            materialsUpdateBase.itemIds.Clear();
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            if (itemManager != null)
            {
                itemManager.GetListItemId();
                materialsUpdateBase.itemIds.AddRange(itemManager.itemIds); // Lấy danh sách itemIds từ ItemManager
            }
        }

        GUILayout.Space(5);
        
        if (materialsUpdateBase.itemIds.Count > 0)
        {
            foreach (var _materialsUpdate in materialsUpdateBase.materialsUpdate)
            {
                int selectedIndex = materialsUpdateBase.itemIds.IndexOf(_materialsUpdate.itemId);
                if (selectedIndex < 0) selectedIndex = 0;   // Nếu không tìm thấy, chọn mục đầu tiên

                selectedIndex = EditorGUILayout.Popup("Item ID", selectedIndex, materialsUpdateBase.itemIds.ToArray());
                _materialsUpdate.itemId = materialsUpdateBase.itemIds[selectedIndex];
            }
        }
        else
        {
            EditorGUILayout.LabelField("No item IDs loaded.");
        }
    }
}
