[System.Serializable]
public class HouseWorld
{
    public string houseId;
    public int level;
    public string houseWorldId;

    public HouseWorld()
    { }

    public HouseWorld(string houseId, int level)
    {
        this.houseId = houseId;
        this.level = level;
    }
}
