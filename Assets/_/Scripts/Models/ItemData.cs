[System.Serializable]
public class ItemData
{
    public string itemId;
    public string name;
    public string type;               // Sử dụng string thay vì enum
    public string description;
    public Attribute attributes;
    public string properties;         // Sử dụng string thay vì enum
    public float createTime;
    public float usingTime;

    public ItemData(Item item)
    {
        this.itemId = item.itemId;
        this.name = item.name;
        this.type = item.type.ToString();               // Chuyển enum thành chuỗi
        this.description = item.description;
        this.attributes = item.attributes;
        this.properties = item.properties.ToString();   // Chuyển enum thành chuỗi
        this.createTime = item.createTime;
        this.usingTime = item.usingTime;
    }
}
