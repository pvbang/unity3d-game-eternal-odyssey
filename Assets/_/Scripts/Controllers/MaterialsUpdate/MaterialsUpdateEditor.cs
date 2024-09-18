using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialsUpdateManager))]
public class MaterialsUpdateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MaterialsUpdateManager materialsUpdateManager = (MaterialsUpdateManager)target;

        GUILayout.Space(10); // Tạo khoảng cách giữa phần Inspector mặc định và nút bấm

        // Nút để tải danh sách itemIds từ Firebase
        if (GUILayout.Button("Load Item IDs from Firebase"))
        {
            materialsUpdateManager.itemIds.Clear();
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            if (itemManager != null)
            {
                itemManager.GetListItemId();
                materialsUpdateManager.itemIds.AddRange(itemManager.itemIds); // Lấy danh sách itemIds từ ItemManager
            }
        }

        GUILayout.Space(5);
        
        if (materialsUpdateManager.itemIds.Count > 0)
        {
            foreach (var _materialsUpdate in materialsUpdateManager.materialsUpdate)
            {
                int selectedIndex = materialsUpdateManager.itemIds.IndexOf(_materialsUpdate.itemId);
                if (selectedIndex < 0) selectedIndex = 0; // Nếu không tìm thấy, chọn mục đầu tiên

                selectedIndex = EditorGUILayout.Popup("Item ID", selectedIndex, materialsUpdateManager.itemIds.ToArray());
                _materialsUpdate.itemId = materialsUpdateManager.itemIds[selectedIndex];
            }
        }
        else
        {
            EditorGUILayout.LabelField("No item IDs loaded.");
        }
    }
}
