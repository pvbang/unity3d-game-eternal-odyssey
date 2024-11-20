using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtribute : MonoBehaviour
{
    public Attribute atribute;

    public int getCurrentHealth()
    {
        return atribute.health.current;
    }

    public void setCurrentHealth(int health)
    {
        atribute.health.current = health;
    }

    public int getMaxHealth()
    {
        return atribute.health.max;
    }

    public int getDamageFrom()
    {
        return atribute.damage.from;
    }

    public int getDamageTo()
    {
        return atribute.damage.to;
    }

    public float getRunSpeed()
    {
        return atribute.runSpeed;
    }
}
