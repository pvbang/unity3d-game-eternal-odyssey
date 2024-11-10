using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandle : MonoBehaviour
{
    public int minDamage = 0;
    public int maxDamage = 0;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantManager.SKILL_TAG))
        {
            Debug.Log("EnemyDamageHandle");
            int damage = Random.Range(minDamage, maxDamage);
        }
    }


    public void SetDamage(int min, int max)
    {
        minDamage = min;
        maxDamage = max;
    }
}
