using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public GameObject questUI;
    private QuestDetailUI questDetailUI;
    public Transform questListContainer;
    public QuestItemSystem questItemSystem;
    public GameObject questSlotPrefab;           // Prefab đại diện cho mỗi quest
    public static bool isOpenQuestUI = false;
    private GameObject selectedQuestItem;
    private Color selectedColor;
    private Color defaultColor;
    public IconManager iconManager;

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
        // Đợi cho đến khi dữ liệu questsList được tải xong
        yield return new WaitUntil(() => QuestItemSystem.questsList.Count > 0);
        DisplayQuestList();
    }

    private void DisplayQuestList()
    {
        // Xóa các quest slot cũ trước khi hiển thị danh sách mới
        foreach (Transform child in questListContainer)
        {
            Destroy(child.gameObject);
        }

        // Lặp qua danh sách questsList và tạo UI cho mỗi quest
        foreach (var quest in QuestItemSystem.questsList)
        {
            GameObject questSlot = Instantiate(questSlotPrefab, questListContainer);
            var questTitle = questSlot.transform.Find("Title").GetComponent<Text>();
            questTitle.text = quest.quest.title;
            var itemShow = questSlot.transform.Find("ItemShow");

            if (itemShow != null)
            {
                var itemIcon = itemShow.Find("Image").GetComponent<Image>();
                if (itemIcon != null)
                {
                    //fetch icon quest
                    switch (quest.quest.type)
                    {
                        case QuestType.NomalQuests:
                            {
                                itemIcon.sprite = iconManager.GetIcon("khoi_dau");
                                break;
                            }
                        case QuestType.IntermediateQuests:
                            {
                                itemIcon.sprite = iconManager.GetIcon("trung_cap");
                                break;
                            }
                        case QuestType.DragonQuests:
                            {
                                itemIcon.sprite = iconManager.GetIcon("danh_rong");
                                break;
                            }
                        case QuestType.FinalQuest:
                            {
                                itemIcon.sprite = iconManager.GetIcon("king_soul");
                                break;
                            }
                        default:
                            {
                                itemIcon.sprite = iconManager.GetIcon("khoi_dau");
                                break;
                            }
                    }
                }
            }

            Button questButton = questSlot.GetComponent<Button>();
            questButton.onClick.AddListener(() => OnQuestItemClicked(quest, questSlot));
        }
    }

    private void OnQuestItemClicked(QuestWorld quest, GameObject questSlot)
    {
        // Hiển thị chi tiết của nhiệm vụ
        if (questDetailUI != null)
        {
            questDetailUI.SetQuestDetails(quest);
        }

        // Đổi màu của QuestItem khi được chọn
        if (selectedQuestItem != null)
        {
            selectedQuestItem.GetComponent<Image>().color = defaultColor;
        }

        selectedQuestItem = questSlot;
        selectedQuestItem.GetComponent<Image>().color = selectedColor;
    }
}
