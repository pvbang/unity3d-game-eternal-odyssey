using System.Collections.Generic;

[System.Serializable]

public class Quest
{
    public string questId;
    public string title;                // Tiêu đề nhiệm vụ
    public string description;          // Mô tả nhiệm vụ
    public Reward reward;               // Phần thưởng khi hoàn thành nhiệm vụ
    public List<string> requirements;         // Điều kiện nhận nhiệm     // Tạm để là ID của nhiệm vụ (thường là nv trước)
    public List<MaterialsUpdate> completionConditions;
    public QuestType type;              // Loại nhiệm vụ (nhiệm vụ bình thường, khởi đầu, trung cấp, đánh rồng, cuối cùng)

    public Quest()
    {}

    public Quest(string title, string description, Reward reward, List<string> requirements, List<MaterialsUpdate> completionConditions, QuestType type)
    {
        this.title = title;
        this.description = description;
        this.reward = reward;
        this.requirements = requirements;
        this.type = type;
        this.completionConditions = completionConditions;
    }

    public Quest(string questId, string title, string description, Reward reward, List<string> requirements, List<MaterialsUpdate> completionConditions, QuestType type)
    {
        this.questId = questId;
        this.title = title;
        this.description = description;
        this.reward = reward;
        this.requirements = requirements;
        this.type = type;
        this.completionConditions = completionConditions;

    }
}