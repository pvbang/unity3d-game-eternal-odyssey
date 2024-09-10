public enum Rare
{
    Normal,
    Rare,
    Legendary
}

public enum Properties
{
    Buff,
    Create
}

[System.Serializable]
public class Item
{
    public string itemId;
    public string name;
    public Rare type;
    public string description;
    public Attribute attributes;
    public Properties properties;
    public float createTime;
    public float usingTime;

    public Item() { }

    public Item(string name, Rare type, string description, Attribute attributes, Properties properties, float createTime, float usingTime)
    {
        this.name = name;
        this.type = type;
        this.description = description;
        this.attributes = attributes;
        this.properties = properties;
        this.createTime = createTime;
        this.usingTime = usingTime;
    }

    public Item(string itemId, string name, Rare type, string description, Attribute attributes, Properties properties, float createTime, float usingTime)
    {
        this.itemId = itemId;
        this.name = name;
        this.type = type;
        this.description = description;
        this.attributes = attributes;
        this.properties = properties;
        this.createTime = createTime;
        this.usingTime = usingTime;
    }
}
