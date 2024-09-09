using System.Collections;
using System.Collections.Generic;
[System.Serializable]

public class HouseFeatureRate
{
    public string house_feature_rate_id;
    public string level;
    public float rate;

    public HouseFeatureRate()
    { }

    public HouseFeatureRate(string level, float rate)
    {
        this.level = level;
        this.rate = rate;
    }
}
