using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManagerUI : MonoBehaviour
{
    public GameObject questUI;

    public static bool isOpenQuestUI = false;
    public void Start()
    {
        questUI.SetActive(false);
    }

    public void Update()
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
}
