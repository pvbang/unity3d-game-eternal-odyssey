using System.Collections.Generic;

[System.Serializable]
public class Weapon
{
    public string weaponId;
    public string name;
    public Level level;
    public Attribute attributes;
    public string description;
    public Effect effect;                               
    public List<MaterialsUpdate> materials;          // List các vật liệu cần để tạo ra vũ khí này
    public Skills skills;
    public Element properties;                       // Thuộc tính của vũ khí (Kim, Mộc, Thủy, Hỏa, Thổ)
    public float createTime;   // (giây)             // Thời gian chế tạo vũ khí này

    public Weapon()
    {}

    public Weapon(string name, Level level, Attribute attributes, string description, Effect effect, List<MaterialsUpdate> materials, Skills skills, Element properties, float createTime)
    {
        this.name = name;
        this.level = level;
        this.attributes = attributes;
        this.description = description;
        this.effect = effect;
        this.materials = materials;
        this.skills = skills;
        this.properties = properties;
        this.createTime = createTime;
    }

    public Weapon(string weaponId, string name, Level level, Attribute attributes, string description, Effect effect, List<MaterialsUpdate> materials, Skills skills, Element properties, float createTime)
    {
        this.weaponId = weaponId;
        this.name = name;
        this.level = level;
        this.attributes = attributes;
        this.description = description;
        this.effect = effect;
        this.materials = materials;
        this.skills = skills;
        this.properties = properties;
        this.createTime = createTime;
    }
}
