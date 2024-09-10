using System.Collections.Generic;

[System.Serializable]
public class Attribute
{
    public Rate health;
    public Rate damage;
    public float resistance;
    public float attackSpeed;
    public float runSpeed;
    public float intelligence;
    public float crit;

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
}
