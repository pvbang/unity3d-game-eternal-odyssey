using System.Collections.Generic;

[System.Serializable]

public class HouseFeature 
{
    public Level level;                                 // Level nhà
    public string description;                     
    public List<HouseFeatureRate> houseFeatureRate;     // Tỷ lệ tạo ra vũ khí (cho từng level của vũ khí)
    public List<MaterialsUpdate> materialUpdates;       // List các vật liệu cần để update nhà lên level này

    public HouseFeature()
    { }

    public HouseFeature(string description, List<HouseFeatureRate> houseFeatureRate)
    {
        this.description = description;
        this.houseFeatureRate = houseFeatureRate;
    }
}
