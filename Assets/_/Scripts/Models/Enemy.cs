[System.Serializable]

public class Enemy 
{
    public string enemyId;
    public string name;                 // Tên quái vật
    public Attribute attributes;        // Chỉ số của quái vật
    public Skills skills;               
    public EnemyType type;              // Loại quái vật
    public Element properties;          // Thuộc tính của quái vật (Kim, Mộc, Thủy, Hỏa, Thổ)
    public Reward rewards;              // Phần thưởng khi đánh quái vật

    public Enemy()
    { }

    public Enemy(string name, Attribute attributes, Skills skills, EnemyType type, Element properties, Reward rewards)
    {
        this.name = name;
        this.attributes = attributes;
        this.skills = skills;
        this.type = type;
        this.properties = properties;
        this.rewards = rewards;
    }

    public Enemy(string enemyId, string name, Attribute attributes, Skills skills, EnemyType type, Element properties, Reward rewards)
    {
        this.enemyId = enemyId;
        this.name = name;
        this.attributes = attributes;
        this.skills = skills;
        this.type = type;
        this.properties = properties;
        this.rewards = rewards;
    }
}
