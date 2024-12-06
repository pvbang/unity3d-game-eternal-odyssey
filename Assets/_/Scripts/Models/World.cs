using System;
using System.Collections.Generic;

[System.Serializable]
public class World 
{
    public string worldId;
    public string name;                         // Tên thế giới
    public string code;                         // Mã để vào thế giới
    public List<QuestWorld> questWorld;         // List các quest đang làm / đã hoàn thành
    public List<HouseWorld> houseWorld;         // List các nhà đã xây + level của nhà đó
    public List<PlayerWorld> playerWorld;       // Thông tin người chơi trong thế giới
    public List<BuildingWorld> buildingWorld;   // Đang xây dựng chi đó (nhà, tạo item, ...)
    public Status status;                       // Trạng thái của thế giới
    public string startAt;                    // Thời gian bắt đầu thế giới
    public string endAt;                      // Thời gian kết thúc thế giới

    public World()
    { }

    public World(string name, string code, List<QuestWorld> questWorld, List<HouseWorld> houseWorld, List<PlayerWorld> playerWorld, List<BuildingWorld> buildingWorld, Status status, string startAt, string endAt)
    {
        this.name = name;
        this.code = code;
        this.questWorld = questWorld;
        this.houseWorld = houseWorld;
        this.playerWorld = playerWorld;
        this.buildingWorld = buildingWorld;
        this.status = status;
        this.startAt = startAt;
        this.endAt = endAt;
    }

    public World(string worldId, string name, string code, List<QuestWorld> questWorld, List<HouseWorld> houseWorld, List<PlayerWorld> playerWorld, List<BuildingWorld> buildingWorld, Status status, string startAt, string endAt)
    {
        this.worldId = worldId;
        this.name = name;
        this.code = code;
        this.questWorld = questWorld;
        this.houseWorld = houseWorld;
        this.playerWorld = playerWorld;
        this.buildingWorld = buildingWorld;
        this.status = status;
        this.startAt = startAt;
        this.endAt = endAt;
    }
}
