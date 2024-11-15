using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailUI : MonoBehaviour
{
    public Text questDescriptionTarget;
    public Text questDescription;
    public IconManager iconManager;
    public IconManager iconEnemys;
    public Transform progressIconsContainer;
    public GameObject progressIconPrefab;
    public Transform rewardIconContainer;
    public GameObject rewardIconPrefab;
    public Transform buttonContainer;
    public void SetQuestDetails(QuestWorld quest)
    {
        // Cập nhật mô tả nhiệm vụ
        questDescription.text = quest.quest.description;
        questDescriptionTarget.text = quest.quest.target_description;

        // Xóa các icon cũ trong progressIconsContainer
        foreach (Transform child in progressIconsContainer)
        {
            Destroy(child.gameObject);
        }

        // Lấy questDetails từ QuestItemSystem để tìm completionConditions
        if (QuestItemSystem.instance.questDetails.TryGetValue(quest.quest.questId, out Quest questDetail))
        {
            // Duyệt qua danh sách completionConditions và tạo icon
            foreach (var condition in questDetail.completionConditions)
            {
                GameObject slot = Instantiate(progressIconPrefab, progressIconsContainer);
                slot.SetActive(true);
                var itemIcon = slot.transform.Find("Icon").GetComponent<Image>();
                // Tạo một GameObject mới để hiển thị icon

                GameObject slotReward = Instantiate(rewardIconPrefab, rewardIconContainer);
                slotReward.SetActive(true);
                var itemIconReward = slotReward.transform.Find("Icon").GetComponent<Image>();
                // Kiểm tra iconManager và iconEnemys để lấy icon dựa trên itemId
                if (iconManager != null && iconManager.GetIcon(condition.itemId))
                {
                    itemIcon.sprite = iconManager.GetIcon(condition.itemId);
                }
                else if (iconEnemys != null && iconEnemys.GetIcon(condition.itemId))
                {
                    itemIcon.sprite = iconEnemys.GetIcon(condition.itemId);
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy icon cho itemId: {condition.itemId}");
                    continue;
                }
                int currentQuantity = 0;
                foreach (var progress in quest.progress_quest_world)
                {
                    if (progress.itemId == condition.itemId)
                    {
                        currentQuantity = progress.quantity;
                        break;
                    }
                }
                var progressQuest = slot.transform.Find("Text").GetComponent<Text>();
                progressQuest.text = $"{currentQuantity}";
                var completionConditions = slot.transform.Find("Text1").GetComponent<Text>();
                completionConditions.text = $"/{condition.quantity}";
            }

            //check reward từ list quests
            foreach (var rewardItem in questDetail.reward.rewardItem)
            {
                GameObject slotReward = Instantiate(rewardIconPrefab, rewardIconContainer);
                slotReward.SetActive(true);
                var itemIconReward = slotReward.transform.Find("Icon").GetComponent<Image>();
                // Kiểm tra iconManager và iconEnemys để lấy icon dựa trên itemId
                if (iconManager != null && iconManager.GetIcon(rewardItem.itemId))
                {
                    itemIconReward.sprite = iconManager.GetIcon(rewardItem.itemId);
                }
                else if (iconEnemys != null && iconEnemys.GetIcon(rewardItem.itemId))
                {
                    itemIconReward.sprite = iconEnemys.GetIcon(rewardItem.itemId);
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy icon cho itemId: {rewardItem.itemId}");
                    continue;
                }
                var progressQuest = slotReward.transform.Find("Quantity").GetComponent<Text>();
                progressQuest.text = $"{rewardItem.quantity}";            
            }

            //check status quest để hiển thị button nhận
            // Kiểm tra trạng thái nhiệm vụ
            Transform buttonTransform = buttonContainer.Find("Complete");
            Button button = buttonTransform.GetComponent<Button>();

            if (quest.status == Status.Complete)
            {
                Debug.Log("Quest Completed");
                button.gameObject.SetActive(false);
            }
            else if (quest.status == Status.InProgess)
            {
                button.gameObject.SetActive(true);
                button.interactable = true;

                bool allConditionsMet = true;
                foreach (var condition in questDetail.completionConditions)
                {
                    int currentQuantity = quest.progress_quest_world.Find(p => p.itemId == condition.itemId)?.quantity ?? 0;
                    if (currentQuantity < condition.quantity)
                    {
                        allConditionsMet = false;
                        break;
                    }
                }

                // Nếu chưa hoàn thành điều kiện thì disable button
                if (!allConditionsMet)
                {
                    button.interactable = false;
                }
            }
            else if (quest.status == Status.NotStarted)
            {
                bool canStartQuest = true;

                foreach (var reqId in questDetail.requirements)
                {
                    // Kiểm tra xem có quest với questId trong questWorld không
                    QuestWorld requiredQuest = QuestItemSystem.instance.GetQuestById(reqId); // Giả sử có phương thức GetQuestById
                    if (requiredQuest == null || requiredQuest.status != Status.Complete)
                    {
                        canStartQuest = false;
                        break;
                    }
                }

                // Hiển thị nút và thiết lập khả năng tương tác dựa trên canStartQuest
                button.gameObject.SetActive(true);
                button.interactable = canStartQuest;

            }

        }
        else
        {
            Debug.LogWarning($"Không tìm thấy thông tin chi tiết cho questId: {quest.quest.questId}");
        }
    }
}
