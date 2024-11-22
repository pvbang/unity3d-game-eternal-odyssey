[System.Serializable]
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
