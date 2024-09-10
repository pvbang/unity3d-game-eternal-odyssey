[System.Serializable]
public class MaterialsUpdate
{
    public string itemId;
    public string quantity;

    public MaterialsUpdate()
    {
    }

    public MaterialsUpdate(string itemId, string quantity)
    {
        this.itemId = itemId;
        this.quantity = quantity;
    }
}
