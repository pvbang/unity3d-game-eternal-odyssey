using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestList))]
public class QuestListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        QuestList questList = (QuestList)target;

        GUILayout.Space(10); // Tạo khoảng cách giữa phần Inspector mặc định và nút bấm

        // Nút để tải danh sách itemIds từ Firebase
        if (GUILayout.Button("Load Item IDs from Firebase"))
        {
            questList.questIds.Clear();
            ItemManager itemManager = FindObjectOfType<ItemManager>();
            if (itemManager != null)
            {
                itemManager.GetListItemId();
                questList.questIds.AddRange(itemManager.itemIds); // Lấy danh sách itemIds từ ItemManager
            }
        }

        GUILayout.Space(5);

        if (questList.questIds.Count > 0)
        {
            foreach (var completion in questList.quest.completionConditions)
            {
                int selectedIndex = questList.questIds.IndexOf(completion.itemId);
                if (selectedIndex < 0) selectedIndex = 0; // Nếu không tìm thấy, chọn mục đầu tiên

                selectedIndex = EditorGUILayout.Popup("Item ID Completion Conditions", selectedIndex, questList.questIds.ToArray());
                completion.itemId = questList.questIds[selectedIndex];
            }
        }
        else
        {
            EditorGUILayout.LabelField("No item IDs loaded.");
        }

        GUILayout.Space(5);

        if (questList.questIds.Count > 0)
        {
            foreach (var rewardItem in questList.quest.reward.rewardItem)
            {
                int selectedIndex = questList.questIds.IndexOf(rewardItem.itemId);
                if (selectedIndex < 0) selectedIndex = 0; // Nếu không tìm thấy, chọn mục đầu tiên

                selectedIndex = EditorGUILayout.Popup("Item ID Reward Item", selectedIndex, questList.questIds.ToArray());
                rewardItem.itemId = questList.questIds[selectedIndex];
            }
        }
        else
        {
            EditorGUILayout.LabelField("No item IDs loaded.");
        }
    }
}
