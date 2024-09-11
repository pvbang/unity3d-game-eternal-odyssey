using System.Collections.Generic;

[System.Serializable]
public class House
{
    public string houseId;
    public string name;
    public string type;
    public Level level; 
    public List<HouseFeature> houseFeatures;    // List các chức năng của từng level nhà
    public float createTime;    // (phút)       // Thời gian xây dựng nhà 

    public House()
    { }

    public House(string name, string type, Level level, List<HouseFeature> houseFeatures, float createTime)
    {
        this.name = name;
        this.type = type;
        this.level = level;
        this.houseFeatures = houseFeatures;
        this.createTime = createTime;
    }

    public House(string houseId, string name, string type, Level level, List<HouseFeature> houseFeatures, float createTime)
    {
        this.houseId = houseId;
        this.name = name;
        this.type = type;
        this.level = level;
        this.houseFeatures = houseFeatures;
        this.createTime = createTime;
    }
}
