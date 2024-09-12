[System.Serializable]

public class Quest
{
    public string questId;
    public string title;                // Tiêu đề nhiệm vụ
    public string description;          // Mô tả nhiệm vụ
    public Reward reward;               // Phần thưởng khi hoàn thành nhiệm vụ
    public string requirements;         // Điều kiện nhận nhiệm     // Tạm để là ID của nhiệm vụ (thường là nv trước)
    public QuestType type;              // Loại nhiệm vụ (nhiệm vụ bình thường, khởi đầu, trung cấp, đánh rồng, cuối cùng)

    public Quest()
    {}

    public Quest(string title, string description, Reward reward, string requirements, QuestType type)
    {
        this.title = title;
        this.description = description;
        this.reward = reward;
        this.requirements = requirements;
        this.type = type;
    }

    public Quest(string questId, string title, string description, Reward reward, string requirements, QuestType type)
    {
        this.questId = questId;
        this.title = title;
        this.description = description;
        this.reward = reward;
        this.requirements = requirements;
        this.type = type;
    }
}