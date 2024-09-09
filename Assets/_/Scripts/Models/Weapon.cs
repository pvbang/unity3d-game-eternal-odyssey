using System.Collections.Generic;

[System.Serializable]
public class Weapon
{
    public string name;
    public int level;
    public List<float> damage;
    public float resistance;
    public float attack_speed;
    public float crit;
    public string description;
    public Effect effect;
    public List<MaterialsUpdate> materials_update;
    public Skills skills;
    public Element type;
    public int time;

    public Weapon()
    {}

    public Weapon(string name, int level, List<float> damage, float resistance, float attack_speed, float crit, string description, Effect effect, List<MaterialsUpdate> materials_update, Skills skills, Element type, int time)
    {
        this.name = name;
        this.level = level;
        this.damage = damage;
        this.resistance = resistance;
        this.attack_speed = attack_speed;
        this.crit = crit;
        this.description = description;
        this.effect = effect;
        this.materials_update = materials_update;
        this.skills = skills;
        this.type = type;
        this.time = time;
    }
}
