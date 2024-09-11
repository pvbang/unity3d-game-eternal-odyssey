[System.Serializable]

public class HouseWorld
{
    public string houseId;                  // ID c?a nhà
    public Level level;                       // Level hi?n t?i c?a nhà

    public HouseWorld()
    { }

    public HouseWorld(string houseId, Level level)
    {
        this.houseId = houseId;
        this.level = level;
    }
}
