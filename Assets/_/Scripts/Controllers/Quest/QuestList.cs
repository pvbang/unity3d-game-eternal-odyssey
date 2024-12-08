using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestList: MonoBehaviour
{
    [Header("Save To Database")]
    public bool save = false;

    [Header("Item Information")]
    public Quest quest;

    [HideInInspector]
    public List<string> questIds = new();   // Danh sách questId từ Firebase
    private void Start()
    {
        if (quest == null)
        {
            return;
        }

        // nếu save = true thì lưu quest này vào database
        if (save)
        {
            // nếu quest này chưa nhập vào id thì sẽ lấy id của gameObject, không thì sẽ lấy id mình nhập vào của item
            if (quest.questId == "")
            {
                quest.questId = gameObject.name;
            }
            // thêm quest vào database
            StartCoroutine(QuestManager.AddQuest(quest));
        }
    }
}
