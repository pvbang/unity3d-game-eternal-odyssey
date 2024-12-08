using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponList))]
public class WeaponManageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WeaponList weaponList = (WeaponList)target;

        GUILayout.Space(10); // Tạo khoảng cách giữa phần Inspector mặc định và nút bấm

        // Nút để tải danh sách itemIds từ Firebase
        if (GUILayout.Button("Load Item for Material"))
        {
            weaponList.weaponIds.Clear();
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            if (itemManager != null)
            {
                itemManager.GetListItemId();
                weaponList.weaponIds.AddRange(itemManager.itemIds); // Lấy danh sách itemIds từ ItemManager
            }
        }

        GUILayout.Space(5);

        if (weaponList.weaponIds.Count > 0)
        {
            foreach (var materials in weaponList.weapon.materials)
            {
                int selectedIndex = weaponList.weaponIds.IndexOf(materials.itemId);
                if (selectedIndex < 0) selectedIndex = 0; // Nếu không tìm thấy, chọn mục đầu tiên

                selectedIndex = EditorGUILayout.Popup("Item ID Completion Conditions", selectedIndex, weaponList.weaponIds.ToArray());
                materials.itemId = weaponList.weaponIds[selectedIndex];
            }
        }
        else
        {
            EditorGUILayout.LabelField("No item IDs loaded.");
        }
    }
}
