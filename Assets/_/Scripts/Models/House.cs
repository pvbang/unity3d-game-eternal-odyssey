[System.Serializable]
public class House
{
    public string name;
    public string type;
    public int level;
    public HouseFeature[] houseFeatures;
    public HouseFeatureRate[] houseFeaturesRate;

    public House()
    { }

    public House(string name, string type, int level, HouseFeature[] houseFeatures, HouseFeatureRate[] houseFeaturesRate)
    {
        this.name = name;
        this.type = type;
        this.level = level;
        this.houseFeatures = houseFeatures;
        this.houseFeaturesRate = houseFeaturesRate;
    }
}
