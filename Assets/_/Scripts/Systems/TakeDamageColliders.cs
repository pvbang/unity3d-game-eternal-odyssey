using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageColliders : MonoBehaviour
{
    public int minDamage;
    public int maxDamage;

    public bool playerDamageToEnemy = false;
    public bool enemyDamageToPlayer = false;

    // Hàm gọi khi 2 vật thể chưa collider bắt đầu va chạm
    //private void OnCollisionEnter(Collider collision)
    //{
    //    Debug.Log("OnTriggerEnter");
    //    if (playerDamageToEnemy)
    //    {
    //        if (collision.CompareTag(ConstantManager.ENEMY_TAG))
    //        {
    //            int damage = Random.Range(minDamage, maxDamage);
    //            Debug.Log("Damage: " + damage);
    //            //collision.GetComponent<EnemyController>().TakeDamage(damage);
    //        }
    //    }

    //    if (enemyDamageToPlayer)
    //    {
    //        if (collision.CompareTag(ConstantManager.PLAYER_TAG))
    //        {
    //            int damage = Random.Range(minDamage, maxDamage);
    //            Debug.Log("Damage: " + damage);
    //            //collision.GetComponent<PlayerController>().TakeDamage(damage);
    //        }
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");

        if (playerDamageToEnemy)
        {
            if (collision.collider.CompareTag(ConstantManager.ENEMY_TAG))
            {
                int damage = Random.Range(minDamage, maxDamage);
                Debug.Log("Damage: " + damage);
                //collision.collider.GetComponent<EnemyController>().TakeDamage(damage);
            }
        }

        if (enemyDamageToPlayer)
        {
            if (collision.collider.CompareTag(ConstantManager.PLAYER_TAG))
            {
                int damage = Random.Range(minDamage, maxDamage);
                Debug.Log("Damage: " + damage);
                //collision.collider.GetComponent<PlayerController>().TakeDamage(damage);
            }
        }
    }

    public void SetDamage(int min, int max)
    {
        minDamage = min;
        maxDamage = max;
    }
}
