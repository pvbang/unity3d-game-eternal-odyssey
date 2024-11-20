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
            Debug.Log("Damage: " + damage);
        }

        if (other.CompareTag(ConstantManager.WEAPON_TAG))
        {
            if ((Input.GetKey(KeyCode.Space) && (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.J))) || (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.J)))
            {
                Debug.Log("EnemyDamageHandle WEAPON_TAG");
                int damage = Random.Range(minDamage, maxDamage);

                Debug.Log("Damage: " + damage);
            }
        }
    }


    public void SetDamage(int min, int max)
    {
        minDamage = min;
        maxDamage = max;
    }
}
