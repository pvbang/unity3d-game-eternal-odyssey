using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public GameObject questUI;
    public QuestDetailUI questDetailUI;
    public Transform questListContainer;
    public QuestItemSystem questItemSystem;
    public GameObject questSlotPrefab;
    public static bool isOpenQuestUI = false;
    private GameObject selectedQuestItem;
    private Color selectedColor;
    private Color defaultColor;
    public IconManager iconManager;
    public GameObject prefabCompleteQuest;

    private void Start()
    {
        questUI.SetActive(false);
        QuestItemSystem.instance.onQuestUpdated += DisplayQuestList;
        StartCoroutine(QuestItemSystem.instance.CompareQuestData());
        StartCoroutine(WaitForQuestDataAndDisplay());

        UnityEngine.ColorUtility.TryParseHtmlString("#0081FF", out selectedColor); // Ví dụ: màu vàng cho item được chọn
        UnityEngine.ColorUtility.TryParseHtmlString("#FFFFFF", out defaultColor);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpenQuestUI = !isOpenQuestUI;
            questUI.SetActive(isOpenQuestUI);

            if (isOpenQuestUI)
            {
                if (!questDetailUI.gameObject.activeSelf)
                {
                    questDetailUI.gameObject.SetActive(true);
                }
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    private IEnumerator WaitForQuestDataAndDisplay()
    {
        yield return new WaitUntil(() => QuestItemSystem.questsList.Count > 0);

        if (!questDetailUI.gameObject.activeSelf)
        {
            questDetailUI.gameObject.SetActive(true);
        }
        DisplayQuestList();
        if (QuestItemSystem.questsList.Count > 0)
        {
            questDetailUI.SetQuestDetails(QuestItemSystem.questsList[0]);
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(questDetailUI.questDescription.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(questDetailUI.questDescriptionTarget.rectTransform);
        }
    }

    private IEnumerator UpdateDefaultQuestLayout()
    {
        yield return null; // Đợi 1 frame để đảm bảo UI được khởi tạo đầy đủ
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(questDetailUI.questDescription.rectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(questDetailUI.questDescriptionTarget.rectTransform);
    }

    private void DisplayQuestList()
    {
        foreach (Transform child in questListContainer)
        {
            Destroy(child.gameObject);
        }

        GameObject firstQuestSlot = null;

        foreach (var quest in QuestItemSystem.questsList)
        {
            GameObject questSlot = Instantiate(questSlotPrefab, questListContainer);
            var content = questSlot.transform.Find("Content");
            var questTitle = content.Find("Title").GetComponent<Text>();
            string questName = ConvertQuestIdToName(quest.quest.questId);
            questTitle.text = $"{questName} - {quest.quest.title}";
            var itemShow = content.Find("ItemShow");

            if (itemShow != null)
            {
                var itemIcon = itemShow.Find("Image").GetComponent<Image>();
                if (itemIcon != null)
                {
                    //fetch icon quest
                    switch (quest.quest.type)
                    {
                        case QuestType.NomalQuests:
                            itemIcon.sprite = iconManager.GetIcon("khoi_dau");
                            break;
                        case QuestType.IntermediateQuests:
                            itemIcon.sprite = iconManager.GetIcon("trung_cap");
                            break;
                        case QuestType.DragonQuests:
                            itemIcon.sprite = iconManager.GetIcon("danh_rong");
                            break;
                        case QuestType.FinalQuest:
                            itemIcon.sprite = iconManager.GetIcon("king_soul");
                            break;
                        default:
                            itemIcon.sprite = iconManager.GetIcon("khoi_dau");
                            break;
                    }
                }
            }

            if (quest.status == Status.Complete)
            {
                GameObject completePrefab = Instantiate(prefabCompleteQuest, questSlot.transform);
                RectTransform rectTransform = completePrefab.GetComponent<RectTransform>();
                rectTransform.anchorMin = new Vector2(1, 0.5f); 
                rectTransform.anchorMax = new Vector2(1, 0.5f);
                rectTransform.pivot = new Vector2(1, 0.5f);
                rectTransform.anchoredPosition = new Vector2(-10, 0);
                rectTransform.sizeDelta = new Vector2(115, 65);
            }


            Button questButton = questSlot.GetComponent<Button>();
            questButton.onClick.AddListener(() => OnQuestItemClicked(quest, questSlot));

            if (firstQuestSlot == null)
            {
                firstQuestSlot = questSlot;
                OnQuestItemClicked(quest, questSlot);
            }
        }
    }

    private string ConvertQuestIdToName(string questId)
    {
        if (questId.StartsWith("quest_"))
        {
            string numberPart = questId.Substring(6);
            if (int.TryParse(numberPart, out int questNumber))
            {
                return $"NV{questNumber}";
            }
        }
        return questId;
    }

    private void OnQuestItemClicked(QuestWorld quest, GameObject questSlot)
    {
        if (questDetailUI != null)
        {
            questDetailUI.SetQuestDetails(quest);
        }

        if (selectedQuestItem != null)
        {
            selectedQuestItem.GetComponent<Image>().color = defaultColor;
        }

        selectedQuestItem = questSlot;
        selectedQuestItem.GetComponent<Image>().color = selectedColor;
    }
}
