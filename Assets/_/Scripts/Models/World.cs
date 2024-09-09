[System.Serializable]
public class World 
{
    public string WorldId;
    public string WorldName;
    public string WorldCode;
    public QuestWorld questWorld;
    public HouseWorld HouseWorld;
    public PlayerWorld PlayerWorld;
    public BuildingWorld BuildingWorld;

    public World()
    { }

    public World(string worldId, string worldName, string worldCode, QuestWorld questWorld, HouseWorld houseWorld, PlayerWorld playerWorld, BuildingWorld buildingWorld)
    {
        WorldId = worldId;
        WorldName = worldName;
        WorldCode = worldCode;
        this.questWorld = questWorld;
        HouseWorld = houseWorld;
        PlayerWorld = playerWorld;
        BuildingWorld = buildingWorld;
    }
}
