using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RewardManager))]
public class RewardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RewardManager rewardManager = (RewardManager)target;

        GUILayout.Space(10); // Tạo khoảng cách giữa phần Inspector mặc định và nút bấm

        // Nút để tải danh sách itemIds từ Firebase
        if (GUILayout.Button("Load Item IDs from Firebase"))
        {
            rewardManager.itemIds.Clear();
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            if (itemManager != null)
            {
                itemManager.GetListItemId();
                rewardManager.itemIds.AddRange(itemManager.itemIds); // Lấy danh sách itemIds từ ItemManager
            }
        }

        GUILayout.Space(5);
        
        if (rewardManager.itemIds.Count > 0)
        {
            foreach (var _reward in rewardManager.reward.rewardItem)
            {
                int selectedIndex = rewardManager.itemIds.IndexOf(_reward.itemId);
                if (selectedIndex < 0) selectedIndex = 0; // Nếu không tìm thấy, chọn mục đầu tiên

                selectedIndex = EditorGUILayout.Popup("Item ID", selectedIndex, rewardManager.itemIds.ToArray());
                _reward.itemId = rewardManager.itemIds[selectedIndex];
            }
        }
        else
        {
            EditorGUILayout.LabelField("No item IDs loaded.");
        }
    }
}
