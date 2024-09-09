
[System.Serializable]
public class HouseFeature 
{
    public string description;
    public HouseFeatureRate houseFeatureRate;

    public HouseFeature()
    { }

    public HouseFeature(string description, HouseFeatureRate houseFeatureRate)
    {
        this.description = description;
        this.houseFeatureRate = houseFeatureRate;
    }
}
