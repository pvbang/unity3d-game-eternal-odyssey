[System.Serializable]

public class PlayerDetails
{
    public string name;                 // Tên ingame của người chơi
    public Attribute attribute;         // Thuộc tính của người chơi
    public string prorerties;           // Hệ của người chơi
    public Level level;                 // Cấp độ của người chơi
    public Stats experience;            // Kinh nghiệm của người chơi
    public string weaponLeft;           // Vũ khí trái
    public string weaponRight;          // Vũ khí phải
    public string skin;                 // Hình dáng của người chơi

    public PlayerDetails()
    {
        level = Level.Level_1;
        experience = new Stats(0, 100);
        attribute = new Attribute();
    }

    public PlayerDetails(string name, Attribute attribute, string prorerties, Level level, Stats experience, string weaponLeft, string weaponRight, string skin)
    {
        this.name = name;
        this.attribute = attribute;
        this.prorerties = prorerties;
        this.level = level;
        this.experience = experience;
        this.weaponLeft = weaponLeft;
        this.weaponRight = weaponRight;
        this.skin = skin;
    }

    public string Name { get => name; set => name = value; }
    public Attribute Attribute { get => attribute; set => attribute = value; }
    public string Prorerties { get => prorerties; set => prorerties = value; }
    public Level Level { get => level; set => level = value; }
    public Stats Experience { get => experience; set => experience = value; }
    public string WeaponLeft { get => weaponLeft; set => weaponLeft = value; }
    public string WeaponRight { get => weaponRight; set => weaponRight = value; }
    public string Skin { get => skin; set => skin = value; }

}
