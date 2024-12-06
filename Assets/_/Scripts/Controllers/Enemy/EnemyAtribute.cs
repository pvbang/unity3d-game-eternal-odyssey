using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtribute : MonoBehaviour
{
    public Attribute atribute;
    private string enemyName = RandomManager.GenerateRandomID();

    public string getEnemyName()
    {
        return enemyName;
    }

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

    public int getResistance()
    {
        return atribute.resistance;
    }


    public float getCritical()
    {
        return atribute.crit;
    }

    public float getAttackSpeed()
    {
        return atribute.attackSpeed;
    }

}
