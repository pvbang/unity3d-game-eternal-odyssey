<<<<<<< HEAD
﻿[System.Serializable]
public class Inventory
{
    public string itemId;
    public int quantity;
    public Item item;

    public Inventory() { }

    public Inventory(string itemId, int quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
    }

    public Inventory(Item item, int quantity)
    {
        this.item = item;
        this.itemId = item.itemId;
        this.quantity = quantity;
    }
}
=======
﻿[System.Serializable]
public class Inventory
{
    public string itemId;  // ID của item trong Firebase
    public int quantity;
    public Item item;  // Thông tin chi tiết về item

    // Constructor mặc định
    public Inventory() { }

    // Constructor với itemId và số lượng
    public Inventory(string itemId, int quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
    }

    // Constructor với item và số lượng
    public Inventory(Item item, int quantity)
    {
        this.item = item;
        this.itemId = item.itemId;
        this.quantity = quantity;
    }
}
>>>>>>> 8c5991d9d9de5b1fae16c33e06f7a66c81a6735d
