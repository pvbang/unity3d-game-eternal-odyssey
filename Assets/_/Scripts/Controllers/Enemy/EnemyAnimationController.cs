using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public Animator enemyAnimator;
    public RealtimeObjectSync realtimeObjectSync;

    [Header("Animation")]
    public bool run = false;
    public bool attack = false;
    public bool skill_1 = false;
    public bool skill_2 = false;
    public bool skill_3 = false;
    public bool skill_4 = false;
    public bool skill_5 = false;
    public bool die = false;
    public bool fly = false;
    public bool down = false;
    public bool dizy = false;
    public bool get_hit = false;

    public void Start()
    {
        if (enemyAnimator == null) enemyAnimator = GetComponent<Animator>();
        if (realtimeObjectSync == null) realtimeObjectSync = transform.parent.GetComponent<RealtimeObjectSync>();
    }

    public void PlayAnimation(string nameAnim)
    {
        if (enemyAnimator == null) return;

        switch (nameAnim)
        {
            case "run":
                if (!run) break;
                enemyAnimator.SetBool("run", true);
                realtimeObjectSync.UpdateAnimation(AnimationName.run);
                break;
            case "attack":
                if (!attack) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("attack");
                realtimeObjectSync.UpdateAnimation(AnimationName.attack);
                break;
            case "skill_1":
                if (!skill_1) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("skill_1");
                realtimeObjectSync.UpdateAnimation(AnimationName.skill_1);
                break;
            case "skill_2":
                if (!skill_2) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("skill_2");
                realtimeObjectSync.UpdateAnimation(AnimationName.skill_2);
                break;
            case "skill_3":
                if (!skill_3) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("skill_3");
                realtimeObjectSync.UpdateAnimation(AnimationName.skill_3);
                break;
            case "skill_4":
                if (!skill_4) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("skill_4");
                realtimeObjectSync.UpdateAnimation(AnimationName.skill_4);
                break;
            case "skill_5":
                if (!skill_5) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("skill_5");
                realtimeObjectSync.UpdateAnimation(AnimationName.skill_5);
                break;

            case "die":
                if (!die) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("die");
                realtimeObjectSync.UpdateAnimation(AnimationName.die);
                break;
            case "fly":
                if (!fly) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("fly");
                realtimeObjectSync.UpdateAnimation(AnimationName.fly);
                break;
            case "down":
                if (!down) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("down");
                realtimeObjectSync.UpdateAnimation(AnimationName.idle);
                break;
            case "dizy":
                if (!dizy) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("dizy");
                realtimeObjectSync.UpdateAnimation(AnimationName.dizzy);
                break;
            case "get_hit":
                if (!get_hit) break;
                StopAnimation("all");
                enemyAnimator.SetTrigger("get_hit");
                realtimeObjectSync.UpdateAnimation(AnimationName.get_hit);
                break;
            default:
                break;
        }
    }


    public void StopAnimation(string nameAnim)
    {
        if (enemyAnimator == null) return;

        switch (nameAnim)
        {
            case "run":
                if (!run) break;
                enemyAnimator.SetBool("run", false);
                break;
            case "all":
                if (run) enemyAnimator.SetBool("run", false);
                if (attack) enemyAnimator.ResetTrigger("attack");
                if (skill_1) enemyAnimator.ResetTrigger("skill_1");
                if (skill_2) enemyAnimator.ResetTrigger("skill_2");
                if (skill_3) enemyAnimator.ResetTrigger("skill_3");
                if (skill_4) enemyAnimator.ResetTrigger("skill_4");
                if (skill_5) enemyAnimator.ResetTrigger("skill_5");
                if (die) enemyAnimator.ResetTrigger("die");
                if (fly) enemyAnimator.ResetTrigger("fly");
                if (down) enemyAnimator.ResetTrigger("down");
                if (dizy) enemyAnimator.ResetTrigger("dizy");
                if (get_hit) enemyAnimator.ResetTrigger("get_hit");
                break;
            default:
                break;
        }
    }


    // lấy độ dài animation bằng tên animation
    public float GetAnimationLength()
    {
        if (enemyAnimator == null) return 0;

        return enemyAnimator.GetCurrentAnimatorStateInfo(0).length;
    }


    // lấy tên animation đang chạy
    public string GetCurrentAnimationName()
    {
        if (enemyAnimator == null || enemyAnimator.GetCurrentAnimatorClipInfo(0).Length == 0)
            return "";

        Debug.Log(enemyAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);

        return enemyAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }


    // check xem animation có đang chạy không
    public bool IsPlayingAnimation(string animationName)
    {
        return enemyAnimator.GetBool(animationName);
    }
}
