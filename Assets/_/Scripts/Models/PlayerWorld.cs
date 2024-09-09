using System.Collections.Generic;

[System.Serializable]
public class PlayerWorld 
{
    public List<string> inventorys;
    public string inGame;
    public string userId;

    public PlayerWorld() { }

    public PlayerWorld(List<string> inventorys, string inGame, string userId)
    {
        this.inventorys = inventorys;
        this.inGame = inGame;
        this.userId = userId;
    }
}
