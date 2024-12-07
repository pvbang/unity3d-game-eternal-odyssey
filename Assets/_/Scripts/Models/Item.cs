using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string itemId;
    public string name;
    public ItemType type;                   // Loại item (Normal, Rare, Legendary)
    public string description;
    public Attribute attributes;            // Thuộc tính của item (dùng để tăng sức mạnh cho nhân vật nếu properties là Buff)
    public ItemProperties properties;       // Các thuộc tính của item (Buff, Create)
    public List<MaterialsUpdate> materials; // List các vật liệu cần để tạo ra item này
    public int createTime;   // (giây)    // Thời gian chế tạo item
    public int usingTime;    // (giây)    // Thời gian item có hiệu lực (nếu item có properties là Buff)

    public Item() { }

    public Item(string name, ItemType type, string description, Attribute attributes, ItemProperties properties, List<MaterialsUpdate> materials, int createTime, int usingTime)
    {
        this.name = name;
        this.type = type;
        this.description = description;
        this.attributes = attributes;
        this.properties = properties;
        this.materials = materials;
        this.createTime = createTime;
        this.usingTime = usingTime;
    }

    public Item(string itemId, string name, ItemType type, string description, Attribute attributes, ItemProperties properties, List<MaterialsUpdate> materials, int createTime, int usingTime)
    {
        this.itemId = itemId;
        this.name = name;
        this.type = type;
        this.description = description;
        this.attributes = attributes;
        this.properties = properties;
        this.materials = materials;
        this.createTime = createTime;
        this.usingTime = usingTime;
    }
}
