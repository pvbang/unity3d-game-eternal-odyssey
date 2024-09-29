using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RewardBase))]
public class RewardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RewardBase rewardBase = (RewardBase)target;

        GUILayout.Space(10); // Tạo khoảng cách giữa phần Inspector mặc định và nút bấm

        // Nút để tải danh sách itemIds từ Firebase
        if (GUILayout.Button("Load Item IDs from Firebase"))
        {
            ItemManager itemManager = FindObjectOfType<ItemManager>(); ;
            rewardBase.itemIds.Clear();
            if (itemManager != null)
            {
                itemManager.GetListItemId();
                rewardBase.itemIds.AddRange(itemManager.itemIds); // Lấy danh sách itemIds từ ItemManager
            }
        }

        GUILayout.Space(5);
        
        if (rewardBase.itemIds.Count > 0)
        {
            foreach (var _reward in rewardBase.reward.rewardItem)
            {
                int selectedIndex = rewardBase.itemIds.IndexOf(_reward.itemId);
                if (selectedIndex < 0) selectedIndex = 0; // Nếu không tìm thấy, chọn mục đầu tiên

                selectedIndex = EditorGUILayout.Popup("Item ID", selectedIndex, rewardBase.itemIds.ToArray());
                _reward.itemId = rewardBase.itemIds[selectedIndex];
            }
        }
        else
        {
            EditorGUILayout.LabelField("No item IDs loaded.");
        }

    }
}
