using System.Collections.Generic;

[System.Serializable]

public class QuestWorld 
{
    public string questId;         // Id của quest
    public Status status;
    public List<MaterialsUpdate> progress_quest_world;
    public Quest quest;

    public QuestWorld() { }

    public QuestWorld(string questId, Status status, List<MaterialsUpdate> progress_quest_world)
    {
        this.questId = questId;
        this.status = status;
        this.progress_quest_world = progress_quest_world;
    }

    public QuestWorld(Quest quest, Status status, List<MaterialsUpdate> progress_quest_world)
    {
        this.status = status;
        this.progress_quest_world = progress_quest_world;
        this.quest = quest;
    }
}
