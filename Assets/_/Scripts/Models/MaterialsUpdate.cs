[System.Serializable]
public class MaterialsUpdate
{
    public string itemId;       // id của item
    public int quantity;        // số lượng cần để có thể update

    public MaterialsUpdate()
    {
    }

    public MaterialsUpdate(string itemId, int quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
    }
}
