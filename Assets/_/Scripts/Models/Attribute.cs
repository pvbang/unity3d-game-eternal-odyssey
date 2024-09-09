[System.Serializable]
public class Attribute
{
    public float health;
    public float strength;
    public float resistance;
    public float speed;
    public float intelligence;
    public float crit;

    public Attribute()
    {
    }

    public Attribute(float health, float strength, float resistance, float speed, float intelligence, float crit)
    {
        this.health = health;
        this.strength = strength;
        this.resistance = resistance;
        this.speed = speed;
        this.intelligence = intelligence;
        this.crit = crit;
    }
}
