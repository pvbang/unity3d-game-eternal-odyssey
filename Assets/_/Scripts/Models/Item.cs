[System.Serializable]
public enum ItemType
{
    Normal,
    Rare,
    Legendary
}

[System.Serializable]
public class Item
{
    public string itemId;
    public string name;
    public ItemType type; // Sử dụng enum ItemType thay vì string
    public string description;
    public Attribute attributes;
    public float rate;
    public string properties; // Buff, Material
    public float time;
    public float usingTime;

    public Item()
    {
    }

    public Item(string name, ItemType type, string description, Attribute attributes, float rate, string properties, float time, float usingTime)
    {
        this.name = name;
        this.type = type; // Sử dụng ItemType thay vì string
        this.description = description;
        this.attributes = attributes;
        this.rate = rate;
        this.properties = properties;
        this.time = time;
        this.usingTime = usingTime;
    }

    public Item(string itemId, string name, ItemType type, string description, Attribute attributes, float rate, string properties, float time, float usingTime)
    {
        this.itemId = itemId;
        this.name = name;
        this.type = type; // Sử dụng ItemType thay vì string
        this.description = description;
        this.attributes = attributes;
        this.rate = rate;
        this.properties = properties;
        this.time = time;
        this.usingTime = usingTime;
    }
}
