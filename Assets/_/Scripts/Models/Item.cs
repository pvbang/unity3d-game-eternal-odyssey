using System.Collections.Generic;

public enum Rare
{
    Normal,
    Rare,
    Legendary
}

public enum Properties
{
    Buff, Create
}

[System.Serializable]
public class Item
{
    public string itemId;
    public string name;
    public Rare type;               // Normal, Rare, Legendary
    public string description;
    public Attribute attributes;
    public Properties properties;   // Buff, Material
    public float createTime;        // Thời gian tạo ra item
    public float usingTime;         // Thời gian sử dụng item

    public Item()
    {
    }

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

    // Thuộc tính chỉ đọc để lấy chuỗi của enum type
    public string TypeString
    {
        get { return type.ToString(); }
    }

    // Thuộc tính chỉ đọc để lấy chuỗi của enum properties
    public string PropertiesString
    {
        get { return properties.ToString(); }
    }
}
