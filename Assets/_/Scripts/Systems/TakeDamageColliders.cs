using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageColliders : MonoBehaviour
{
    public int minDamage = 0;
    public int maxDamage = 0;

    private string objectTag;

    private void Awake()
    {
        // lấy ra tag của object này
        Debug.Log("Tag: " + gameObject.tag);
        objectTag = gameObject.tag;
    }


    private void TakeDamagePlayer()
    {
        Debug.Log("TakeDamagePlayer");
        int damage = Random.Range(minDamage, maxDamage);
        PlayerSystemController.instance.TakeDamage(damage);
    }

    private void TakeDamageEnemy()
    {
        Debug.Log("TakeDamageEnemy");
        int damage = Random.Range(minDamage, maxDamage);
    }


    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("OnCollisionEnter");

    //    if (collision.collider.CompareTag(ConstantManager.SKILL_TAG))
    //    {
    //        if (objectTag == ConstantManager.PLAYER_TAG)
    //        {
    //            TakeDamagePlayer();
    //        }
    //        else if (objectTag == ConstantManager.ENEMY_TAG)
    //        {
    //            TakeDamageEnemy();
    //        }
    //    }
    //    else if (collision.collider.CompareTag(ConstantManager.PLAYER_TAG))
    //    {
    //        if (objectTag == ConstantManager.SKILL_TAG)
    //        {
    //            TakeDamagePlayer();
    //        }
    //    }
    //    else if (collision.collider.CompareTag(ConstantManager.ENEMY_TAG))
    //    {
    //        if (objectTag == ConstantManager.SKILL_TAG)
    //        {
    //            TakeDamageEnemy();
    //        }
    //    }
    //}


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstantManager.SKILL_TAG))
        {
            if (objectTag == ConstantManager.PLAYER_TAG)
            {
                TakeDamagePlayer();
            }
            else if (objectTag == ConstantManager.ENEMY_TAG)
            {
                TakeDamageEnemy();
            }
        }
        else if (other.CompareTag(ConstantManager.PLAYER_TAG))
        {
            if (objectTag == ConstantManager.SKILL_TAG)
            {
                TakeDamagePlayer();
            }
        }
        else if (other.CompareTag(ConstantManager.ENEMY_TAG))
        {
            if (objectTag == ConstantManager.SKILL_TAG)
            {
                TakeDamageEnemy();
            }
        }
    }


    public void SetDamage(int min, int max)
    {
        minDamage = min;
        maxDamage = max;
    }
}
