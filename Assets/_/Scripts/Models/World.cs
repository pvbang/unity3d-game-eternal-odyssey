using System.Collections.Generic;

[System.Serializable]
public class World 
{
    public string worldId;
    public string name;                 // Tên thế giới
    public string code;                 // Mã để vào thế giới
    public QuestWorld questWorld;       // List các quest đang làm / đã hoàn thành
    public HouseWorld houseWorld;       // List các nhà đã xây + level của nhà đó
    public PlayerWorld playerWorld;     // Thông tin người chơi trong thế giới
    public BuildingWorld buildingWorld; // Chưa hiểu lắm :v

    public World()
    { }

    public World(string name, string code, QuestWorld questWorld, HouseWorld houseWorld, PlayerWorld playerWorld, BuildingWorld buildingWorld)
    {
        this.name = name;
        this.code = code;
        this.questWorld = questWorld;
        this.houseWorld = houseWorld;
        this.playerWorld = playerWorld;
        this.buildingWorld = buildingWorld;
    }

    public World(string worldId, string name, string code, QuestWorld questWorld, HouseWorld houseWorld, PlayerWorld playerWorld, BuildingWorld buildingWorld)
    {
        this.worldId = worldId;
        this.name = name;
        this.code = code;
        this.questWorld = questWorld;
        this.houseWorld = houseWorld;
        this.playerWorld = playerWorld;
        this.buildingWorld = buildingWorld;
    }
}
