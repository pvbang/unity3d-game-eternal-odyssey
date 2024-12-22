using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageValue : MonoBehaviour
{
    public int minDamage = 5;
    public int maxDamage = 10;
    public float critical = 0.0f;

    public bool isWeapons = false;


    bool IsLucky(float luckyRate)
    {
        float randomValue = Random.Range(0f, 100f); 
        return randomValue < luckyRate; 
    }

    public int GetDamage()
    {
        if (isWeapons)
        {
            if (!(Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.J)))
            {
                return 0;
            }
        }

        if (IsLucky(critical))
        {
            return Random.Range(minDamage, maxDamage) * 2;
        }
        else
        {
            return Random.Range(minDamage, maxDamage);
        }
    }

}
