using System.Collections.Generic;

[System.Serializable]

public class HouseFeatureRate
{
    public Level level;         // Level vũ khí
    public float rate;          // Tỷ lệ tạo ra vũ khí (cho từng level của vũ khí)

    public HouseFeatureRate()
    { }

    public HouseFeatureRate(Level level, float rate)
    {
        this.level = level;
        this.rate = rate;
    }
}

/*
 
Nhà rèn	
Cấp 0	
Cấp 1	    90% vũ khí lv1,         10% vũ khí lv2,         0% vũ khí lv3
Cấp 2	    29.5% vũ khí lv1,       70% vũ khí lv2,         0.5% vũ khí lv3,
Cấp 3	    2% vũ khí lv1,          55% vũ khí lv2,         43% vũ khí lv3

*/