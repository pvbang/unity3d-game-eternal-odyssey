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

    public GameObject prefabComplete;
    public Transform containerComplete;
    public Button buttonComplete;
    public Button buttonAccept;

    private GameObject instantiatedComplete;

    public void SetQuestDetails(QuestWorld quest)
    {
        questDescription.text = quest.quest.description;
        questDescriptionTarget.text = quest.quest.target_description;

        if (!gameObject.activeSelf)
        {
            Debug.LogWarning("QuestDetailUI is inactive! Activating...");
            gameObject.SetActive(true);
        }

        foreach (Transform child in progressIconsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in rewardIconContainer)
        {
            Destroy(child.gameObject);
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(questDescription.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(questDescriptionTarget.rectTransform);

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

            Debug.Log($"Số lượng item cũ trước khi dọn: {rewardIconContainer.childCount}");
            foreach (Transform child in rewardIconContainer)
            {
                Destroy(child.gameObject);
            }
            Debug.Log("Đã dọn sạch các item cũ trong rewardIconContainer.");


            //check reward từ list quests
            if (questDetail.reward.rewardItem != null && questDetail.reward.rewardItem.Count > 0)
            {
                foreach (var rewardItem in questDetail.reward.rewardItem)
                {
                    Debug.Log($"Creating reward icon for itemId: {rewardItem.itemId} with quantity: {rewardItem.quantity}");
                    GameObject slotReward = Instantiate(rewardIconPrefab, rewardIconContainer);
                    slotReward.SetActive(true);
                    var itemIconReward = slotReward.transform.Find("Icon").GetComponent<Image>();

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
            }
            else
            {
                Debug.LogWarning($"No rewards found for quest {quest.quest.questId}");
            }

            //check status quest để hiển thị button nhận
            if (quest.status == Status.Complete)
            {
                // Hiển thị prefabComplete và ẩn các nút
                if (instantiatedComplete == null)
                {
                    instantiatedComplete = Instantiate(prefabComplete, containerComplete);

                    RectTransform rectTransform = instantiatedComplete.GetComponent<RectTransform>();
                    rectTransform.anchorMin = new Vector2(0.5f, 0);
                    rectTransform.anchorMax = new Vector2(0.5f, 0);
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    rectTransform.anchoredPosition = new Vector2(0, 60);
                    rectTransform.sizeDelta = new Vector2(150, 115);
                    rectTransform.localScale = Vector3.one;
                }
                instantiatedComplete.SetActive(true);
                buttonComplete.gameObject.SetActive(false);
                buttonAccept.gameObject.SetActive(false);
            }
            else
            {
                if (instantiatedComplete != null)
                {
                    instantiatedComplete.SetActive(false);
                }
                
                if (quest.status == Status.InProgess)
                {
                    buttonComplete.gameObject.SetActive(true);
                    buttonAccept.gameObject.SetActive(false);

                    bool canReturnQuest = true;

                    foreach (var condition in quest.quest.completionConditions)
                    {
                        int currentQuantity = quest.progress_quest_world.Find(p => p.itemId == condition.itemId)?.quantity ?? 0;

                        if (currentQuantity < condition.quantity)
                        {
                            canReturnQuest = false;
                            break;
                        }
                    }

                    buttonComplete.interactable = canReturnQuest;

                    var buttonImage = buttonComplete.GetComponent<Image>();
                    buttonImage.color = canReturnQuest ? new Color(1f, 1f, 1f, 1f) : new Color(0.7f, 0.7f, 0.7f, 1f);

                    // Xử lý sự kiện nút "Trả nhiệm vụ"
                    buttonComplete.onClick.RemoveAllListeners();
                    buttonComplete.onClick.AddListener(() =>
                    {
                        QuestWorldManager.CompleteQuest(quest.quest.questId, PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID), (success, message) =>
                        {
                            if (success)
                            {
                                Debug.Log($"Trả nhiệm vụ thành công: {quest.quest.questId}");
                                quest.status = Status.Complete; // Cập nhật trạng thái nhiệm vụ

                                // Làm mới giao diện, hiển thị trạng thái hoàn thành
                                SetQuestDetails(quest);
                            }
                            else
                            {
                                Debug.LogError($"Lỗi trả nhiệm vụ: {message}");
                            }
                        });
                    });
                }

                else if (quest.status == Status.NotStarted)
                {
                    buttonComplete.gameObject.SetActive(false);
                    buttonAccept.gameObject.SetActive(true);

                    bool canStartQuest = true;

                    foreach (var reqId in quest.quest.requirements)
                    {
                        QuestWorld requiredQuest = QuestItemSystem.instance.GetQuestById(reqId);
                        if (requiredQuest == null || requiredQuest.status != Status.Complete)
                        {
                            canStartQuest = false;
                            break;
                        }
                    }

                    buttonAccept.interactable = canStartQuest;
                    buttonAccept.GetComponent<Image>().color = canStartQuest ? new Color(1f, 1f, 1f, 1f) : new Color(0.7f, 0.7f, 0.7f, 1f);

                    // Xử lý sự kiện nút "Nhận nhiệm vụ"
                    buttonAccept.onClick.RemoveAllListeners();
                    buttonAccept.onClick.AddListener(() =>
                    {
                        QuestWorldManager.AcceptQuest(quest.quest.questId, PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID), (success, message) =>
                        {
                            if (success)
                            {
                                Debug.Log($"Nhận nhiệm vụ thành công: {quest.quest.questId}");
                                QuestWorld newQuest = quest;
                                newQuest.status = Status.InProgess;

                                SetQuestDetails(newQuest);
                            }
                            else
                            {
                                Debug.LogError($"Lỗi nhận nhiệm vụ: {message}");
                            }
                        });
                    });
                }

            }
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy thông tin chi tiết cho questId: {quest.quest.questId}");
        }
    }
}
