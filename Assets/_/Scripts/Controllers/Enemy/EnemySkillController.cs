using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillController : MonoBehaviour
{
    public BulletSkill bulletSkill;

    private void Start()
    {
        if (bulletSkill == null)
            bulletSkill = GetComponent<BulletSkill>();
    }

    public void Attack(int _minDamage, int _maxDamage, float _critical)
    {
        bulletSkill.BulletAttack(_minDamage, _maxDamage, _critical);
    }
}
