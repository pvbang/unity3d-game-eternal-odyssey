using System.Collections.Generic;

[System.Serializable]
public class PlayerWorld 
{
    public string userId;                   // ID của user
    public List<string> inventorys;         // List các item trong túi
    public string inGame;                   // Trạng thái của người chơi (đang chơi, nghỉ, offline)

    public PlayerWorld() { }

    public PlayerWorld(string userId, List<string> inventorys, string inGame)
    {
        this.userId = userId;
        this.inventorys = inventorys;
        this.inGame = inGame;
    }
}
