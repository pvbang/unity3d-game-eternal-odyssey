[System.Serializable]

public class PlayerFigure
{
    public string bell;             // Chuông
    public string cloth;            // Quần áo
    public string crown;            // Vương miện
    public string face;             // Khuôn mặt
    public string glove;            // Găng tay
    public string hair;             // Tóc
    public string helmet;           // Mũ bảo hiểm
    public string shoes;            // Giày
    public string shoulder;         // Áo choàng
    public string pelvis;           // Túi đồ đeo lưng

    public PlayerFigure()
    {
    }

    public PlayerFigure(string bell, string cloth, string crown, string face, string glove, string hair, string helmet, string shoes, string shoulder, string pelvis)
    {
        this.bell = bell;
        this.cloth = cloth;
        this.crown = crown;
        this.face = face;
        this.glove = glove;
        this.hair = hair;
        this.helmet = helmet;
        this.shoes = shoes;
        this.shoulder = shoulder;
        this.pelvis = pelvis;
    }

    public string Bell { get => bell; set => bell = value; }
    public string Cloth { get => cloth; set => cloth = value; }
    public string Crown { get => crown; set => crown = value; }
    public string Face { get => face; set => face = value; }
    public string Glove { get => glove; set => glove = value; }
    public string Hair { get => hair; set => hair = value; }
    public string Helmet { get => helmet; set => helmet = value; }
    public string Shoes { get => shoes; set => shoes = value; }
    public string Shoulder { get => shoulder; set => shoulder = value; }
    public string Pelvis { get => pelvis; set => pelvis = value; }


}
