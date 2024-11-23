using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSkill : MonoBehaviour
{
    public float destroyTime = 5.0f;

    public GameObject bulletObject;
    public Transform bulletPoint;
    public float bulletSpeed = 20.0f;

    public void BulletAttack(int _minDamage, int _maxDamage, float _critical)
    {
        GameObject bullet = Instantiate(bulletObject, bulletPoint.transform.position, bulletPoint.transform.rotation);

        TakeDamageValue takeDamageValue = bullet.GetComponent<TakeDamageValue>();
        if (takeDamageValue != null)
        {
            takeDamageValue.minDamage = _minDamage;
            takeDamageValue.maxDamage = _maxDamage;
            takeDamageValue.critical = _critical;
        }

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(bulletPoint.forward * bulletSpeed, ForceMode.Impulse);
        Destroy(bullet, destroyTime);
    }
}
