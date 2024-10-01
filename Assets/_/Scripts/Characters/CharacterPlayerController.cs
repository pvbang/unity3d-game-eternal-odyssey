//using System;
//using UnityEngine;

//public class CharacterPlayerController : MonoBehaviour
//{
//    private Animator playerAnimator;
//    public Transform charactersObject;

//    public bool isPressKey = false;

//    private string idle = "idle";
//    private string stop = "stop";

//    private string walk_up = "walk_up";
//    //private string walk_left = "walk_left";
//    //private string walk_right = "walk_right";

//    private string run_up = "run_up";
//    //private string run_down = "run_down";

//    private string sprint = "sprint";

//    private string jump = "jump";
//    private string jump_attack = "jump_attack";

//    private string attack = "attack";
//    private string combo = "combo";

//    private string defend = "defend";


//    private void Start()
//    {
//        if (charactersObject == null)
//        {
//            charactersObject = transform.Find("Characters");
//        }

//        CharacterObjectController characterObjectController = charactersObject.GetComponent<CharacterObjectController>();
//        if (characterObjectController != null)
//        {
//            playerAnimator = characterObjectController.GetActiveCharacterAnimator();
//        }
//    }


//    void Update()
//    {
//        GetKeyInput();
//    }


//    // Hàm kiểm tra tổ hợp phím
//    private bool IsKeyHeld(params KeyCode[] keys)
//    {
//        foreach (KeyCode key in keys)
//        {
//            if (Input.GetKey(key)) return true;
//        }
//        return false;
//    }

//    // Hàm kiểm tra tổ hợp phím thả ra
//    private bool IsKeyReleased(params KeyCode[] keys)
//    {
//        foreach (KeyCode key in keys)
//        {
//            if (Input.GetKeyUp(key)) return true;
//        }
//        return false;
//    }

//    // Hàm lấy input từ bàn phím và điều khiển trạng thái
//    public void GetKeyInput()
//    {
//        // Kiểm tra nếu đang nhấn các phím điều hướng
//        bool isMoving = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
//                        Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

//        // Kiểm tra nếu đang nhấn Shift
//        bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

//        // Nếu đang di chuyển và giữ Shift -> Chạy nhanh
//        if (isMoving && isShiftHeld)
//        {
//            Debug.Log("Up + Shift");
//            TriggerAnimation(sprint);
//        }
//        // Nếu chỉ đang di chuyển mà không giữ Shift -> Đi bộ
//        else if (isMoving)
//        {
//            Debug.Log("Up");
//            TriggerAnimation(walk_up);
//        }
//        // Nhảy và tấn công (Space + Chuột trái)
//        else if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.Mouse0))
//        {
//            Debug.Log("Jump Attack");
//            TriggerAnimation(jump_attack);
//        }
//        // Nhảy
//        else if (Input.GetKey(KeyCode.Space))
//        {
//            TriggerAnimation(jump);
//        }
//        // Tấn công (Chuột trái)
//        else if (Input.GetKey(KeyCode.Mouse0))
//        {
//            TriggerAnimation(attack);
//        }
//        // Combo (Chuột phải)
//        else if (Input.GetKey(KeyCode.Mouse1))
//        {
//            TriggerAnimation(combo);
//        }
//        // Phòng thủ (Ctrl)
//        else if (IsKeyHeld(KeyCode.LeftControl, KeyCode.RightControl))
//        {
//            TriggerAnimation(defend);
//        }
//        else
//        {
//            // Nếu không nhấn phím nào, reset tất cả trạng thái
//            if (isPressKey)  // Chỉ reset nếu trước đó có nhấn phím
//            {
//                isPressKey = false;
//                Debug.Log("Resetting all triggers to idle");
//                ResetAllTriggers();
//            }

//            // Đặt trạng thái nhân vật về `idle`
//            playerAnimator.SetBool(idle, true);
//        }

//        // Dừng lại khi thả phím
//        if (IsKeyReleased(
//                KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow,
//                KeyCode.W, KeyCode.A, KeyCode.D, KeyCode.S,
//                KeyCode.LeftShift, KeyCode.RightShift, KeyCode.LeftAlt, KeyCode.RightAlt,
//                KeyCode.Space, KeyCode.LeftControl, KeyCode.RightControl,
//                KeyCode.Mouse0, KeyCode.Mouse1))
//        {
//            isPressKey = false;
//            playerAnimator.SetTrigger(stop);
//            ResetAllTriggers();  // Reset tất cả trigger khi thả phím
//        }
//    }


//    // Hàm gọi trigger và cập nhật trạng thái
//    private void TriggerAnimation(string triggerName)
//    {
//        if (!isPressKey)
//        {
//            isPressKey = true;
//            playerAnimator.SetBool(idle, false);
//            playerAnimator.SetTrigger(triggerName);
//        }
//    }

//    private void ResetAllTriggers()
//    {
//        playerAnimator.ResetTrigger(stop);
//        playerAnimator.ResetTrigger(walk_up);
//        playerAnimator.ResetTrigger(run_up);
//        playerAnimator.ResetTrigger(sprint);
//        playerAnimator.ResetTrigger(jump);
//        playerAnimator.ResetTrigger(jump_attack);
//        playerAnimator.ResetTrigger(attack);
//        playerAnimator.ResetTrigger(combo);
//        playerAnimator.ResetTrigger(defend);
//    }

//}

using System;
using UnityEngine;

public class CharacterPlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    public Transform charactersObject;

    private bool isPressKey = false;

    // Quản lý trạng thái bằng enum thay cho string
    private enum CharacterState
    {
        Idle,
        Walk,
        Sprint,
        Jump,
        JumpAttack,
        Attack,
        Combo,
        Defend
    }

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
                        Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow);

        bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Quản lý trạng thái tiếp theo
        CharacterState nextAction = CharacterState.Idle;

        // Đặt các điều kiện hành động theo thứ tự ưu tiên
        if (isMoving && isShiftHeld)
        {
            nextAction = CharacterState.Sprint;
        }
        else if (isMoving)
        {
            nextAction = CharacterState.Walk;
        }
        else if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.Mouse0))
        {
            nextAction = CharacterState.JumpAttack;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            nextAction = CharacterState.Jump;
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            nextAction = CharacterState.Attack;
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            nextAction = CharacterState.Combo;
        }
        else if (IsKeyHeld(KeyCode.LeftControl, KeyCode.RightControl))
        {
            nextAction = CharacterState.Defend;
        }

        // Nếu hành động thay đổi, gọi hàm chuyển trạng thái
        if (nextAction != currentAction)
        {
            currentAction = nextAction;
            SetCharacterState(currentAction);
        }

        // Dừng lại khi thả phím và không nhấn phím nào khác
        if (IsKeyReleased(KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow,
                          KeyCode.W, KeyCode.A, KeyCode.D, KeyCode.S, KeyCode.LeftShift, KeyCode.RightShift,
                          KeyCode.LeftAlt, KeyCode.RightAlt, KeyCode.Space, KeyCode.LeftControl, KeyCode.RightControl,
                          KeyCode.Mouse0, KeyCode.Mouse1))
        {
            isPressKey = false;
            currentAction = CharacterState.Idle;
            playerAnimator.SetTrigger("stop");
            ResetAllTriggers();
            playerAnimator.SetBool("idle", true);
        }
    }

    // Hàm quản lý trạng thái Animator
    private void SetCharacterState(CharacterState state)
    {
        isPressKey = true;
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
            default:
                playerAnimator.SetBool("idle", true);
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
    }
}

