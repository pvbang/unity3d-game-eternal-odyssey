using System.Collections.Generic;

[System.Serializable]
public class Weapon
{
    public string weaponId;
    public string name;
    public int level;
    public Attribute attribute;
    public string description;
    public Effect effect;
    public List<MaterialsUpdate> materials_update;
    public Skills skills;
    public Element type;
    public int time;

    public Weapon()
    {}

    public Weapon(string name, int level, Attribute attribute, string description, Effect effect, List<MaterialsUpdate> materials_update, Skills skills, Element type, int time)
    {
        this.name = name;
        this.level = level;
        this.attribute = attribute;
        this.description = description;
        this.effect = effect;
        this.materials_update = materials_update;
        this.skills = skills;
        this.type = type;
        this.time = time;
    }

    public Weapon(string weaponId, string name, int level, Attribute attribute, string description, Effect effect, List<MaterialsUpdate> materials_update, Skills skills, Element type, int time)
    {
        this.weaponId = weaponId;
        this.name = name;
        this.level = level;
        this.attribute = attribute;
        this.description = description;
        this.effect = effect;
        this.materials_update = materials_update;
        this.skills = skills;
        this.type = type;
        this.time = time;
    }
}
