using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [Header("Quản lý Animation")]
    public Animator playerAnimator;

    [Header("List Skill")]

    [Header("Base Skill")]
    public bool baseAttack = false;
    public int baseAttackTimes = 3;         
    public GameObject baseAttackObject;
    public Transform baseAttackPoint;

    [Header("Shield Skill")]
    public bool shieldSkill = false;
    public GameObject shieldSkillObject;
    public Transform shieldSkillPoint;

    [Header("Skill 1")]
    public bool skill_1 = false;
    public GameObject skill_1_Object;
    public Transform skill_1_Point;

    [Header("Bullet Skill")]
    public bool bulletAttack = false;
    public BulletSkill bulletSkill;

    private bool isCtrlHeld = false;


    private void Start()
    {
        if (playerAnimator == null) playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        isCtrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
    }

    private void InstantiateSkill(GameObject skillObject, Transform skillPoint)
    {
        GameObject skillAttack = Instantiate(skillObject, skillPoint.transform.position, skillPoint.transform.rotation);

        Destroy(skillAttack, playerAnimator.GetCurrentAnimatorStateInfo(0).length);
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


    public void PlayShield()
    {
        if (shieldSkill == false || shieldSkillObject == null || shieldSkillPoint == null) return;

        GameObject skillAttack = Instantiate(shieldSkillObject, shieldSkillPoint.transform.position, shieldSkillPoint.transform.rotation);

        // Gọi hàm xử lý skill
        HandleSkillAttack(skillAttack);


        //if (shieldSkill == false || shieldSkillObject == null || shieldSkillPoint == null) return;
        //InstantiateSkill(shieldSkillObject, shieldSkillPoint);
    }

    public void PlaySkill1()
    {
        if (skill_1 == false || skill_1_Object == null || skill_1_Point == null) return;
        InstantiateSkill(skill_1_Object, skill_1_Point);
    }

    public void PlayBulletSkill()
    {
        if (bulletAttack == false || bulletSkill == null) return;
        PlayerDetails playerDetails = PlayerSystemController.instance.playerAtributeUI.GetPlayerDetails();
        if (playerDetails != null)
        {
            Attribute attribute = playerDetails.attribute;
            bulletSkill.BulletAttack(attribute.damage.from, attribute.damage.to, attribute.crit);
        }
        
    }

    // Kiểm tra trạng thái isCtrlHeld và xử lý skillAttack
    private void HandleSkillAttack(GameObject skillAttack)
    {
        StartCoroutine(CheckCtrlHeldAndDestroy(skillAttack));
    }

    private IEnumerator CheckCtrlHeldAndDestroy(GameObject skillAttack)
    {
        // Lấy thời lượng animation hiện tại
        float animationDuration = playerAnimator.GetCurrentAnimatorStateInfo(0).length;

        // Tính thời gian bắt đầu
        float startTime = Time.time;

        // Kiểm tra liên tục trong khoảng thời gian của animation
        while (Time.time - startTime < animationDuration)
        {
            // Nếu phím Ctrl không được nhấn nữa, xóa ngay skill
            if (!isCtrlHeld)
            {
                Destroy(skillAttack);
                yield break; // Thoát coroutine
            }
            yield return null; // Chờ frame tiếp theo
        }

        // Hết thời gian animation thì xóa skill
        Destroy(skillAttack);
    }

}
