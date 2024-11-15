using Firebase.Auth;
using System;
using UnityEngine;
using StarterAssets;

public class CharacterPlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    public Transform charactersObject;
    public ThirdPersonController playerController;

    // Biến để theo dõi trạng thái hành động hiện tại
    private CharacterState currentAction = CharacterState.Idle;

    private void Start()
    {
        if (charactersObject == null)
        {
            charactersObject = transform.Find("Characters");
        }

        CharacterObjectController characterObjectController = charactersObject.GetComponent<CharacterObjectController>();
        if (characterObjectController != null)
        {
            playerAnimator = characterObjectController.GetActiveCharacterAnimator();
        }
    }

    void Update()
    {
        if (InventoryManager.isOpenInventory)
        {
            return;
        }
        GetKeyInput();
    }

    private bool IsKeyHeld(params KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKey(key)) return true;
        }
        return false;
    }

    private bool IsKeyReleased(params KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyUp(key)) return true;
        }
        return false;
    }

    // Hàm lấy input từ bàn phím và điều khiển trạng thái
    private void GetKeyInput()
    {
        // Kiểm tra các trạng thái di chuyển
        bool isMoving = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                        Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
                        Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool isAltHeld = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        bool isCtrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        // Quản lý trạng thái tiếp theo
        CharacterState nextAction = CharacterState.Idle;

        // Đặt các điều kiện hành động theo thứ tự ưu tiên
        if (isMoving && isShiftHeld)
        {
            float stamina = PlayerPrefs.GetFloat(ConstantManager.PLAYER_STAMINA, 100f);
            if (stamina > 0.5)
            {
                nextAction = CharacterState.Sprint;
                playerController.SprintSpeed = 10f;
            }
            else
            {
                nextAction = CharacterState.Walk;
                playerController.SprintSpeed = 3f;
            }
        }
        else if (Input.GetKey(KeyCode.R) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            nextAction = CharacterState.RollUp;
        }
        else if (Input.GetKey(KeyCode.R) && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            nextAction = CharacterState.RollDown;
        }
        else if (Input.GetKey(KeyCode.R) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            nextAction = CharacterState.RollLeft;
        }
        else if (Input.GetKey(KeyCode.R) && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            nextAction = CharacterState.RollRight;
        }
        else if (isAltHeld && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            nextAction = CharacterState.DashUp;
        }
        else if (isAltHeld && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        {
            nextAction = CharacterState.DashDown;
        }
        else if (isAltHeld && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)))
        {
            nextAction = CharacterState.DashLeft;
        }
        else if (isAltHeld && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)))
        {
            nextAction = CharacterState.DashRight;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            nextAction = CharacterState.Slide;
        }
        else if (isMoving)
        {
            nextAction = CharacterState.Walk;
        }
        else if (Input.GetKey(KeyCode.Space) && (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.J)))
        {
            nextAction = CharacterState.JumpAttack;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            nextAction = CharacterState.Jump;
        }
        else if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.J))
        {
            nextAction = CharacterState.Combo;
        }
        //else if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.I))
        //{
        //    nextAction = CharacterState.Combo;
        //}
        else if (isCtrlHeld)
        {
            nextAction = CharacterState.Defend;
        }

        // Nếu hành động thay đổi, gọi hàm chuyển trạng thái
        if (nextAction != currentAction)
        {
            SetCharacterState(nextAction);
        }

        // Dừng lại khi thả phím và không nhấn phím nào khác
        if (IsKeyReleased(KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow,
                          KeyCode.W, KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.J, KeyCode.I, KeyCode.R, KeyCode.Q,
                          KeyCode.LeftShift, KeyCode.RightShift,
                          KeyCode.LeftAlt, KeyCode.RightAlt, 
                          KeyCode.LeftControl, KeyCode.RightControl,
                          KeyCode.Space, 
                          KeyCode.Mouse0, KeyCode.Mouse1))
        {
            playerAnimator.SetTrigger("stop");
            ResetAllTriggers();
            playerAnimator.SetBool("idle", true);
            currentAction = CharacterState.Idle;
        }
    }

    // Hàm quản lý trạng thái Animator
    public void SetCharacterState(CharacterState state)
    {
        playerAnimator.SetBool("idle", false);

        // Reset tất cả trigger trước khi set trigger mới
        ResetAllTriggers();

        switch (state)
        {
            case CharacterState.Walk:
                playerAnimator.SetTrigger("run_up");
                currentAction = CharacterState.Walk;
                break;
            case CharacterState.Sprint:
                playerAnimator.SetTrigger("sprint");
                currentAction = CharacterState.Sprint;
                break;
            case CharacterState.Jump:
                playerAnimator.SetTrigger("jump");
                currentAction = CharacterState.Jump;
                break;
            case CharacterState.JumpAttack:
                playerAnimator.SetTrigger("jump_attack");
                currentAction = CharacterState.JumpAttack;
                break;
            case CharacterState.Attack:
                playerAnimator.SetTrigger("attack");
                currentAction = CharacterState.Attack;
                break;
            case CharacterState.Combo:
                playerAnimator.SetTrigger("combo");
                currentAction = CharacterState.Combo;
                break;
            case CharacterState.Defend:
                playerAnimator.SetTrigger("defend");
                currentAction = CharacterState.Defend;
                break;
            case CharacterState.DefendHit:
                playerAnimator.SetTrigger("defend_hit");
                currentAction = CharacterState.DefendHit;
                break;
            case CharacterState.RollUp:
                playerAnimator.SetTrigger("roll_up");
                currentAction = CharacterState.RollUp;
                break;
            case CharacterState.RollDown:
                playerAnimator.SetTrigger("roll_back");
                currentAction = CharacterState.RollDown;
                break;
            case CharacterState.RollLeft:
                playerAnimator.SetTrigger("roll_left");
                currentAction = CharacterState.RollLeft;
                break;
            case CharacterState.RollRight:
                playerAnimator.SetTrigger("roll_right");
                currentAction = CharacterState.RollRight;
                break;
            case CharacterState.Slide:
                playerAnimator.SetTrigger("slide");
                currentAction = CharacterState.Slide;
                break;
            case CharacterState.DashUp:
                playerAnimator.SetTrigger("dash_up");
                currentAction = CharacterState.DashUp;
                break;
            case CharacterState.DashDown:
                playerAnimator.SetTrigger("dash_back");
                currentAction = CharacterState.DashDown;
                break;
            case CharacterState.DashLeft:
                playerAnimator.SetTrigger("dash_left");
                currentAction = CharacterState.DashLeft;
                break;
            case CharacterState.DashRight:
                playerAnimator.SetTrigger("dash_right");
                currentAction = CharacterState.DashRight;
                break;
            case CharacterState.Die:
                playerAnimator.SetTrigger("die");
                currentAction = CharacterState.Die;
                break;
            case CharacterState.DieRecovered:
                playerAnimator.SetTrigger("die_recovered");
                currentAction = CharacterState.DieRecovered;
                break;
            case CharacterState.Dizzy:
                playerAnimator.SetTrigger("dizzy");
                currentAction = CharacterState.Dizzy;
                break;
            case CharacterState.GetHit:
                playerAnimator.SetTrigger("get_hit");
                currentAction = CharacterState.GetHit;
                break;
            case CharacterState.EquipmentChange:
                playerAnimator.SetTrigger("equipment_change");
                currentAction = CharacterState.EquipmentChange;
                break;
            case CharacterState.Victory:
                playerAnimator.SetTrigger("victory");
                currentAction = CharacterState.Victory;
                break;

            default:
                playerAnimator.SetBool("idle", true);
                currentAction = CharacterState.Idle;
                break;

        }
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

