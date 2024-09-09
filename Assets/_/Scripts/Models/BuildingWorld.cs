using System;
[System.Serializable]
public class BuildingWorld 
{
    public string typeId;
    public DateTime startAt;
    public DateTime endAt;
    public Status status;

    public BuildingWorld()
    { }

    public BuildingWorld(string typeId, DateTime startAt, DateTime endAt, Status status)
    {
        this.typeId = typeId;
        this.startAt = startAt;
        this.endAt = endAt;
        this.status = status;
    }
}
