using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class RealtimePlayerSync : MonoBehaviour
{
    public Transform playerTransform;

    public Animator playerAnimator;
    public SkinCharacterInit skinCharacterInit;

    // Các biến lưu trạng thái cho nội suy
    private Vector3 currentTargetPosition;
    private Quaternion currentTargetRotation;

    private float positionSmoothTime = 0.25f; // Thời gian để chuyển mượt
    private float rotationSmoothTime = 0.25f; // Thời gian để quay mượt

    private CharacterState currentAction = CharacterState.Idle;

    /////////////////////////////////////////////////////////////////////////////////////////
    // LOCATION

    // Hàm set vị trí cho player
    private void Start()
    {
        // Khởi tạo vị trí mục tiêu ban đầu
        if (playerTransform != null)
        {
            currentTargetPosition = playerTransform.localPosition;
            currentTargetRotation = playerTransform.localRotation;
        }
    }

    private void Update()
    {
        // Nội suy vị trí
        playerTransform.localPosition = Vector3.Lerp(playerTransform.localPosition, currentTargetPosition, Time.deltaTime / positionSmoothTime);

        // Nội suy góc quay
        playerTransform.localRotation = Quaternion.Lerp(playerTransform.localRotation, currentTargetRotation, Time.deltaTime / rotationSmoothTime);
    }

    // Hàm set vị trí cho player với nội suy
    public void SetLocation(Vector3 position, Vector3 rotation)
    {
        currentTargetPosition = position;
        currentTargetRotation = Quaternion.Euler(rotation);

        Debug.Log($"[SetLocation] Target Position: {position}, Target Rotation: {rotation}");
    }

    /////////////////////////////////////////////////////////////////////////////////////////
    // SKIN

    // Hàm set skin cho player
    public void SetSkin(string skin)
    {
        skinCharacterInit.SetSkin(skin);
    }


    /////////////////////////////////////////////////////////////////////////////////////////
    // ANIMATION

    // Hàm quản lý trạng thái Animator
    public void SetCharacterState(CharacterState state)
    {
        if (playerAnimator == null) return;

        if (state == currentAction)
        {
            return;
        }

        playerAnimator.SetBool("idle", false);

        // Reset tất cả trigger trước khi set trigger mới
        ResetAllTriggers();

        switch (state)
        {
            case CharacterState.Walk:
                playerAnimator.SetTrigger("run_up");
                break;
            case CharacterState.Sprint:
                playerAnimator.SetTrigger("sprint");
                break;
            case CharacterState.Jump:
                playerAnimator.SetTrigger("jump");
                break;
            case CharacterState.JumpAttack:
                playerAnimator.SetTrigger("jump_attack");
                break;
            case CharacterState.Attack:
                playerAnimator.SetTrigger("attack");
                break;
            case CharacterState.Combo:
                playerAnimator.SetTrigger("combo");
                break;
            case CharacterState.Defend:
                playerAnimator.SetTrigger("defend");
                break;
            case CharacterState.DefendHit:
                playerAnimator.SetTrigger("defend_hit");
                break;
            case CharacterState.RollUp:
                playerAnimator.SetTrigger("roll_up");
                break;
            case CharacterState.RollDown:
                playerAnimator.SetTrigger("roll_back");
                break;
            case CharacterState.RollLeft:
                playerAnimator.SetTrigger("roll_left");
                break;
            case CharacterState.RollRight:
                playerAnimator.SetTrigger("roll_right");
                break;
            case CharacterState.Slide:
                playerAnimator.SetTrigger("slide");
                break;
            case CharacterState.DashUp:
                playerAnimator.SetTrigger("dash_up");
                break;
            case CharacterState.DashDown:
                playerAnimator.SetTrigger("dash_back");
                break;
            case CharacterState.DashLeft:
                playerAnimator.SetTrigger("dash_left");
                break;
            case CharacterState.DashRight:
                playerAnimator.SetTrigger("dash_right");
                break;
            case CharacterState.Die:
                playerAnimator.SetTrigger("die");
                break;
            case CharacterState.DieRecovered:
                playerAnimator.SetTrigger("die_recovered");
                break;
            case CharacterState.Dizzy:
                playerAnimator.SetTrigger("dizzy");
                break;
            case CharacterState.GetHit:
                playerAnimator.SetTrigger("get_hit");
                break;
            case CharacterState.EquipmentChange:
                playerAnimator.SetTrigger("equipment_change");
                break;
            case CharacterState.Victory:
                playerAnimator.SetTrigger("victory");
                break;

            default:
                playerAnimator.SetBool("idle", true);
                break;

        }

        Debug.Log("SetCharacterState: " + state);

        //playerAnimator.SetTrigger("stop");
        //ResetAllTriggers();
        //playerAnimator.SetBool("idle", true);
    }

    // Hàm reset toàn bộ các trigger
    private void ResetAllTriggers()
    {
        playerAnimator.ResetTrigger("stop");
        playerAnimator.ResetTrigger("run_up");
        playerAnimator.ResetTrigger("sprint");
        playerAnimator.ResetTrigger("jump");
        playerAnimator.ResetTrigger("jump_attack");
        playerAnimator.ResetTrigger("attack");
        playerAnimator.ResetTrigger("combo");
        playerAnimator.ResetTrigger("defend");
        playerAnimator.ResetTrigger("defend_hit");
        playerAnimator.ResetTrigger("roll_up");
        playerAnimator.ResetTrigger("roll_back");
        playerAnimator.ResetTrigger("roll_left");
        playerAnimator.ResetTrigger("roll_right");
        playerAnimator.ResetTrigger("slide");
        playerAnimator.ResetTrigger("dash_up");
        playerAnimator.ResetTrigger("dash_back");
        playerAnimator.ResetTrigger("dash_left");
        playerAnimator.ResetTrigger("dash_right");
        playerAnimator.ResetTrigger("die");
        playerAnimator.ResetTrigger("die_recovered");
        playerAnimator.ResetTrigger("dizzy");
        playerAnimator.ResetTrigger("get_hit");
        playerAnimator.ResetTrigger("equipment_change");
        playerAnimator.ResetTrigger("victory");
    }
}
