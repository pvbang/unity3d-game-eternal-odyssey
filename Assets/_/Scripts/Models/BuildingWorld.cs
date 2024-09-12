using System;
[System.Serializable]
public class BuildingWorld                  // Đang xây dựng item hoặc nhà nào đó trong thế giới
{
    public string buildingId;                // ID của item hoặc nhà
    public DateTime startAt;                // Thời gian bắt đầu xây dựng
    public DateTime endAt;                  // Thời gian kết thúc xây dựng
    public Status status;                   // Trạng thái xây dựng 

    public BuildingWorld()
    { }

    public BuildingWorld(string buildingId, DateTime startAt, DateTime endAt, Status status)
    {
        this.buildingId = buildingId;
        this.startAt = startAt;
        this.endAt = endAt;
        this.status = status;
    }
}
