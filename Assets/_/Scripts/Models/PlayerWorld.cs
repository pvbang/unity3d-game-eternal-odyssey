using System.Collections.Generic;

[System.Serializable]
public class PlayerWorld 
{
    public string userId;                       // ID của user
    public List<Inventory> inventorys;          // List các item trong túi
    public PlayerStatus playerStatus;           // Trạng thái của người chơi (đang chơi, nghỉ, offline)

    public PlayerWorld() { }

    public PlayerWorld(string userId, List<Inventory> inventorys, PlayerStatus playerStatus)
    {
        this.userId = userId;
        this.inventorys = inventorys;
        this.playerStatus = playerStatus;
    }
}
