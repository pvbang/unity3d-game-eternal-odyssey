using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public GameObject questUI;
    public Transform questListContainer;
    public QuestItemSystem questItemSystem;
    public GameObject questSlotPrefab;           // Prefab đại diện cho mỗi quest
    public Text questTitleText;       // Tiêu đề của nhiệm vụ
    public Text questDescriptionText;
    public static bool isOpenQuestUI = false;
    private void Start()
    {
        questUI.SetActive(false);

        // Gọi CompareQuestData để load dữ liệu từ Firebase
        QuestItemSystem.instance.onQuestUpdated += DisplayQuestList;

        // Bắt đầu tải dữ liệu quest
        StartCoroutine(QuestItemSystem.instance.CompareQuestData());
        // Hiển thị danh sách nhiệm vụ sau khi tải xong
        StartCoroutine(WaitForQuestDataAndDisplay());
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
            var questTitle = questSlot.transform.Find("Title").GetComponent<TextMeshProUGUI>();
            if (questTitle != null && quest.quest != null)
                questTitle.text = quest.quest.title;

            var questDescription = questSlot.transform.Find("DescriptionTarget").GetComponent<TextMeshProUGUI>();
            if (questTitle != null && quest.quest != null)
                questDescription.text = quest.quest.target_description;

            var viewButton = questSlot.transform.Find("ButtonView").GetComponent<Button>();
            //viewButton.onClick.AddListener(() => OnViewQuestDetails(quest));
        }
    }

    //private void OnViewQuestDetails(QuestWorld quest)
    //{
    //    // Cập nhật chi tiết của quest lên UI
    //    questTitleText.text = quest.quest.title;
    //    questDescriptionText.text = quest.quest.target_description;
    //}
}
