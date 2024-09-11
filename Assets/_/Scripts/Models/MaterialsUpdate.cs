[System.Serializable]
public class MaterialsUpdate
{
    public string itemId;       // id của item
    public string quantity;     // số lượng cần để có thể update

    public MaterialsUpdate()
    {
    }

    public MaterialsUpdate(string itemId, string quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
    }
}
