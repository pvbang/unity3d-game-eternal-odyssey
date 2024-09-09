using System.Collections.Generic;

public enum ListProperties
{
    Minions,
    Elite,
    Champion,
    MiniBoss,
    Boss,
    FinalBoss
}

[System.Serializable]
public class Enemy 
{
    public string name;
    public float health;
    public float speed;
    public float strength;
    public float resistance;
    public float intelligence;
    public float crit;
    public Skills skill;
    public Element type;
    public ListProperties properties;
    public List<string> rewards;

    public Enemy()
    { }

    public Enemy(string name, float health, float speed, float strength, float resistance, float intelligence, float crit, Skills skill, Element type, ListProperties properties, List<string> rewards)
    {
        this.name = name;
        this.health = health;
        this.speed = speed;
        this.strength = strength;
        this.resistance = resistance;
        this.intelligence = intelligence;
        this.crit = crit;
        this.skill = skill;
        this.type = type;
        this.properties = properties;
        this.rewards = rewards;
    }
}
