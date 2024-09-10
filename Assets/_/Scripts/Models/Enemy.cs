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
    public string enemyId;
    public string name;
    public Attribute attribute;
    public Skills skill;
    public Element type;
    public ListProperties properties;
    public List<string> rewards;

    public Enemy()
    { }

    public Enemy(string name, Attribute attribute, Skills skill, Element type, ListProperties properties, List<string> rewards)
    {
        this.name = name;
        this.attribute = attribute;
        this.skill = skill;
        this.type = type;
        this.properties = properties;
        this.rewards = rewards;
    }

    public Enemy(string enemyId, string name, Attribute attribute, Skills skill, Element type, ListProperties properties, List<string> rewards)
    {
        this.enemyId = enemyId;
        this.name = name;
        this.attribute = attribute;
        this.skill = skill;
        this.type = type;
        this.properties = properties;
        this.rewards = rewards;
    }
}
