[System.Serializable]

public class PlayerDetails
{
    public string name;                 // Tên ingame của người chơi
    public Attribute attribute;         // Thuộc tính của người chơi
    public string prorerties;           // Hệ của người chơi
    public Level level;                 // Cấp độ của người chơi
    public string weaponLeft;           // Vũ khí trái
    public string weaponRight;          // Vũ khí phải
    public PlayerFigure playerFigure;   // Hình dáng của người chơi

    public PlayerDetails()
    {
    }

    public PlayerDetails(string name, Attribute attribute, string prorerties, Level level, string weaponLeft, string weaponRight, PlayerFigure playerFigure)
    {
        this.name = name;
        this.attribute = attribute;
        this.prorerties = prorerties;
        this.level = level;
        this.weaponLeft = weaponLeft;
        this.weaponRight = weaponRight;
        this.playerFigure = playerFigure;
    }

    public string Name { get => name; set => name = value; }
    public Attribute Attribute { get => attribute; set => attribute = value; }
    public string Prorerties { get => prorerties; set => prorerties = value; }
    public Level Level { get => level; set => level = value; }
    public string WeaponLeft { get => weaponLeft; set => weaponLeft = value; }
    public string WeaponRight { get => weaponRight; set => weaponRight = value; }
    public PlayerFigure PlayerFigure { get => playerFigure; set => playerFigure = value; }
        
}
