using System.Collections.Generic;

public enum Properties
{
    Buff, Create
}
[System.Serializable]
public class Item
{
    public string itemId;
    public string name;
    public Rare type; // Sử dụng enum ItemType thay vì string
    public string description;
    public Attribute attributes;
    public List<float> rate; //List này có 2 giá trị VD 75%-85%
    public Properties properties; // Buff, Material
    public List<string> location; //Địa điểm có thể tìm thấy //id cây hoặc mỏ quặng...
    public float createTime;
    public float usingTime;

    public Item()
    {
    }

    public Item(string name, Rare type, string description, Attribute attributes, List<float> rate, Properties properties, float createTime, float usingTime)
    {
        this.name = name;
        this.type = type;
        this.description = description;
        this.attributes = attributes;
        this.rate = rate;
        this.properties = properties;
        this.createTime = createTime;
        this.usingTime = usingTime;
    }
}
