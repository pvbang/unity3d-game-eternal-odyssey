using System.Collections.Generic;
[System.Serializable]
public class QuestWorld 
{
    public string questId;
    public Status status;

    public QuestWorld() 
    { }

    public QuestWorld(string questId, Status status)
    {
        this.questId = questId;
        this.status = status;
    }
}
