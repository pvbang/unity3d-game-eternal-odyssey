[System.Serializable]

public class Attribute
{
    public Rate health;                 // Máu
    public Rate damage;                 // Sát thương
    public float resistance;            // Chống chịu
    public float attackSpeed;           // Tốc độ tấn công
    public float runSpeed;              // Tốc độ chạy
    public float intelligence;          // Trí tuệ ? cái ni để làm chi hè
    public float crit;                  // Tỉ lệ chí mạng

    public Attribute()
    {
    }

    public Attribute(Rate health, Rate damage, float resistance, float attackSpeed, float runSpeed, float intelligence, float crit)
    {
        this.health = health;
        this.damage = damage;
        this.resistance = resistance;
        this.attackSpeed = attackSpeed;
        this.runSpeed = runSpeed;
        this.intelligence = intelligence;
        this.crit = crit;
    }

    public Rate Health { get => health; set => health = value; }
    public Rate Damage { get => damage; set => damage = value; }
    public float Resistance { get => resistance; set => resistance = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public float Intelligence { get => intelligence; set => intelligence = value; }
    public float Crit { get => crit; set => crit = value; }

}
