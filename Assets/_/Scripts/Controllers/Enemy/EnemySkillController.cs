using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillController : MonoBehaviour
{
    [Header("Quản lý Animation")]
    public EnemyAnimationController enemyAnimatorController;

    [Header("Skill tầm xa")]
    public bool bulletAttack = false;
    public BulletSkill bulletSkill;

    [Header("List Skill")]

    [Header("Base Skill")]
    public bool baseAttack = false;
    public int baseAttackTimes = 3;          // số lần đánh thường để sử dụng skill
    public GameObject baseAttackObject;
    public Transform baseAttackPoint;

    [Header("Skill 1")]
    public bool skill_1 = false;
    public GameObject skill_1_Object;
    public Transform skill_1_Point;

    [Header("Skill 2")]
    public bool skill_2 = false;
    public GameObject skill_2_Object;
    public Transform skill_2_Point;

    [Header("Skill 3")]
    public bool skill_3 = false;
    public GameObject skill_3_Object;
    public Transform skill_3_Point;

    [Header("Skill 4")]
    public bool skill_4 = false;
    public GameObject skill_4_Object;
    public Transform skill_4_Point;

    [Header("Skill 5")]
    public bool skill_5 = false;
    public GameObject skill_5_Object;
    public Transform skill_5_Point;

    [Header("Skill 6")]
    public bool skill_6 = false;
    public GameObject skill_6_Object;
    public Transform skill_6_Point;

    [Header("Skill 7")]
    public bool skill_7 = false;
    public GameObject skill_7_Object;
    public Transform skill_7_Point;

    private int baseAttackCounter = 0; // Đếm số lần đánh thường

    private int _minDamage = 1;
    private int _maxDamage = 1;
    private float _critical = 0;


    private void Start()
    {
        if (enemyAnimatorController == null) enemyAnimatorController = GetComponent<EnemyAnimationController>();
    }

    public float Attack(int _minDamage, int _maxDamage, float _critical)
    {
        if (baseAttack)
        {
            // nếu còn animation attack đang chạy thì không thực hiện
            if (enemyAnimatorController.IsPlayingAnimation("attack")) return enemyAnimatorController.GetAnimationLength();

            this._minDamage = _minDamage;
            this._maxDamage = _maxDamage;
            this._critical = _critical;

            if (baseAttackCounter >= baseAttackTimes)
            {
                baseAttackCounter = 0; // Reset số lần đánh thường
                return UseRandomSkill(_minDamage, _maxDamage, _critical); // Sử dụng skill ngẫu nhiên
            } else
            {
                enemyAnimatorController.PlayAnimation("attack");
                baseAttackCounter++;
            }

            return enemyAnimatorController.GetAnimationLength();
        }

        else return 0;
    }

    private float UseRandomSkill(int _minDamage, int _maxDamage, float _critical)
    {
        List<int> activeSkills = new List<int>();
        if (skill_1) activeSkills.Add(1);
        if (skill_2) activeSkills.Add(2);
        if (skill_3) activeSkills.Add(3);
        if (skill_4) activeSkills.Add(4);
        if (skill_5) activeSkills.Add(5);
        if (skill_6) activeSkills.Add(6);
        if (skill_7) activeSkills.Add(7);

        if (activeSkills.Count == 0)
        {
            return Attack(_minDamage, _maxDamage, _critical);
        }

        int randomIndex = Random.Range(0, activeSkills.Count);
        int chosenSkill = activeSkills[randomIndex];

        string animationName = "skill_" + chosenSkill;

        enemyAnimatorController.PlayAnimation(animationName);
        Debug.Log("Sử dụng skill: " + chosenSkill);

        return enemyAnimatorController.GetAnimationLength();
    }


    private void InstantiateSkill(GameObject skill_Object, Transform skill_Point)
    {
        GameObject skillAttack = Instantiate(skill_Object, skill_Point.transform.position, skill_Point.transform.rotation);
        TakeDamageValue takeDamageValue = skillAttack.GetComponent<TakeDamageValue>();
        if (takeDamageValue != null)
        {
            takeDamageValue.minDamage = _minDamage;
            takeDamageValue.maxDamage = _maxDamage;
            takeDamageValue.critical = _critical;
        }

        Destroy(skillAttack, enemyAnimatorController.GetAnimationLength());
    }


    public void PlayNormalAttack()
    {
        if (baseAttack == false || baseAttackObject == null || baseAttackPoint == null) return;
        InstantiateSkill(baseAttackObject, baseAttackPoint);
    }

    public void PlayNormalAttackRandomPos()
    {
        if (baseAttack == false || baseAttackObject == null || baseAttackPoint == null) return;

        Vector3 randomPosition = new Vector3(
            baseAttackPoint.position.x + Random.Range(-2.5f, 2.5f),
            baseAttackPoint.position.y + Random.Range(-2.5f, 2.5f),
            baseAttackPoint.position.z + Random.Range(-1f, 1f)
        );

        GameObject tempBaseAttackPoint = new GameObject("TempBaseAttackPoint");
        tempBaseAttackPoint.transform.position = randomPosition;

        InstantiateSkill(baseAttackObject, tempBaseAttackPoint.transform);

        Destroy(tempBaseAttackPoint);
    }


    public void PlaySkill1()
    {
        if (skill_1 == false || skill_1_Object == null || skill_1_Point == null) return;
        InstantiateSkill(skill_1_Object, skill_1_Point);
    }

    public void PlaySkill2()
    {
        if (skill_2 == false || skill_2_Object == null || skill_2_Point == null) return;
        InstantiateSkill(skill_2_Object, skill_2_Point);
    }

    public void PlaySkill3()
    {
        if (skill_3 == false || skill_3_Object == null || skill_3_Point == null) return;
        InstantiateSkill(skill_3_Object, skill_3_Point);
    }

    public void PlaySkill4()
    {
        if (skill_4 == false || skill_4_Object == null || skill_4_Point == null) return;
        InstantiateSkill(skill_4_Object, skill_4_Point);
    }

    public void PlaySkill5()
    {
        if (skill_5 == false || skill_5_Object == null || skill_5_Point == null) return;
        InstantiateSkill(skill_5_Object, skill_5_Point);
    }

    public void PlaySkill6()
    {
        if (skill_6 == false || skill_6_Object == null || skill_6_Point == null) return;
        InstantiateSkill(skill_6_Object, skill_6_Point);
    }

    public void PlaySkill7()
    {
        if (skill_7 == false || skill_7_Object == null || skill_7_Point == null) return;
        InstantiateSkill(skill_7_Object, skill_7_Point);
    }

}
