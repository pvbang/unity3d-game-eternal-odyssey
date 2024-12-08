using System.Collections.Generic;

[System.Serializable]
public class PlayerWorld 
{
    public string userId;                       // ID của user
    public List<Inventory> inventorys;          // List các item trong túi
    public PlayerStatus playerStatus;           // Trạng thái của người chơi (đang chơi, nghỉ, offline)
    public Role role;                           // Vai trò của người chơi trong thế giới
    public PlayerDetails playerDetails;         // Chi tiết người chơi

    public PlayerWorld() { }

    public PlayerWorld(string userId, List<Inventory> inventorys, PlayerStatus playerStatus, Role role, PlayerDetails playerDetails)
    {
        this.userId = userId;
        this.inventorys = inventorys;
        this.playerStatus = playerStatus;
        this.role = role;
        this.playerDetails = playerDetails;
    }
}
