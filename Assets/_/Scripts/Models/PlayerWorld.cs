using System.Collections.Generic;

[System.Serializable]
public class PlayerWorld 
{
    public string userId;                       // ID của user
    public List<Inventory> inventorys;          // List các item trong túi
    public PlayerStatus playerStatus;           // Trạng thái của người chơi (đang chơi, nghỉ, offline)
    public Role role;                           // Vai trò của người chơi trong thế giới
    public PlayerDetails playerDetails;         // Chi tiết người chơi
    public Location position;                   // Vị trí của người chơi
    public Location rotation;                   // Hướng nhìn của người chơi
    public CharacterState characterState;       // Trạng thái animation của nhân vật

    public PlayerWorld() { 
        this.position = new Location(143.18f, 11.43f, 136.3f);
        this.rotation = new Location(0, 0, 0);
        this.characterState = CharacterState.Idle;
    }

    public PlayerWorld(string userId, List<Inventory> inventorys, PlayerStatus playerStatus, Role role, PlayerDetails playerDetails, Location position, Location rotation, CharacterState characterState)
    {
        this.userId = userId;
        this.inventorys = inventorys;
        this.playerStatus = playerStatus;
        this.role = role;
        this.playerDetails = playerDetails;
        this.position = position;
        this.rotation = rotation;
        this.characterState = characterState;
    }
}
