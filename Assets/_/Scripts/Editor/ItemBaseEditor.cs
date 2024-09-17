using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

[CustomEditor(typeof(ItemBase))]
public class ItemBaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ItemBase itemBase = (ItemBase)target;

        GUILayout.Space(10); // Tạo khoảng cách giữa phần Inspector mặc định và nút bấm

        // Nút để tải danh sách itemIds từ Firebase
        if (GUILayout.Button("Load Item IDs from Firebase"))
        {
            itemBase.itemIds.Clear();
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            if (itemManager != null)
            {
                itemManager.GetListItemId();
                itemBase.itemIds.AddRange(itemManager.itemIds); // Lấy danh sách itemIds từ ItemManager
            }
        }

        GUILayout.Space(5); // Tạo khoảng cách giữa phần Inspector mặc định và nút bấm

        // Hiển thị danh sách itemId cho mỗi vật liệu trong materials
        if (itemBase.itemIds.Count > 0)
        {
            foreach (var material in itemBase.item.materials)
            {
                int selectedIndex = itemBase.itemIds.IndexOf(material.itemId);
                if (selectedIndex < 0) selectedIndex = 0; // Nếu không tìm thấy, chọn mục đầu tiên

                selectedIndex = EditorGUILayout.Popup("Material ID", selectedIndex, itemBase.itemIds.ToArray());
                material.itemId = itemBase.itemIds[selectedIndex];
            }
        }
        else
        {
            EditorGUILayout.LabelField("No item IDs loaded.");
        }
    }
}
