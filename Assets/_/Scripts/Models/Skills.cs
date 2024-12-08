[System.Serializable]

public class Skills
{
    public string name;         // Tên skill
    public float damage;        // Sát thương của skill

    public Skills()
    {}

    public Skills(string name, float damage)
    {
        this.name = name;
        this.damage = damage;
    }
}
