using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestDetailUI : MonoBehaviour
{
    public Text questDescriptionTarget;
    public Text questDescription;
    public Text questProgress;

    public void SetQuestDetails(QuestWorld quest)
    {
        questDescription.text = quest.quest.description;
        questDescriptionTarget.text = quest.quest.target_description;
    }
}
