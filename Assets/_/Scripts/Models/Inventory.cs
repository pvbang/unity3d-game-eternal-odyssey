[System.Serializable]

public class Inventory
{
    public string itemId;       // id của item
    public int quantity;        // số lượng item hiện có

    public Inventory()
    {
    }

    public Inventory(string itemId, int quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
    }


}
