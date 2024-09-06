[System.Serializable]
public class MaterialsUpdate
{
    public string materialId;
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

    public MaterialsUpdate(string materialId, string itemId, string quantity)
    {
        this.materialId = materialId;
        this.itemId = itemId;
        this.quantity = quantity;
    }
}
