using UnityEngine;

public class CharacterPlayerController : MonoBehaviour
{
    private Animator playerAnimator;
    public Transform charactersObject;

    public bool isPressKey = false;

    private string idle = "idle";
    private string stop = "stop";

    private string walk_up = "walk_up";
    //private string walk_left = "walk_left";
    //private string walk_right = "walk_right";

    private string run_up = "run_up";
    //private string run_down = "run_down";

    private string sprint = "sprint";

    private string jump = "jump";
    private string jump_attack = "jump_attack";

    private string attack = "attack";
    private string combo = "combo";

    private string defend = "defend";



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


    // Hàm kiểm tra tổ hợp phím
    private bool IsKeyHeld(params KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKey(key)) return true;
        }
        return false;
    }

    // Hàm kiểm tra tổ hợp phím thả ra
    private bool IsKeyReleased(params KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyUp(key)) return true;
        }
        return false;
    }

    // Hàm lấy input từ bàn phím và điều khiển trạng thái
    public void GetKeyInput()
    {
        // Di chuyển
        if (IsKeyHeld(KeyCode.UpArrow, KeyCode.W, KeyCode.DownArrow, KeyCode.S, KeyCode.LeftArrow, KeyCode.A, KeyCode.RightArrow, KeyCode.D))
        {
            TriggerAnimation(walk_up);
        }
        // Chạy nhanh (Shift + UpArrow hoặc Shift + W)
        else if (IsKeyHeld(KeyCode.LeftShift, KeyCode.RightShift) && IsKeyHeld(KeyCode.UpArrow, KeyCode.W))
        {
            TriggerAnimation(run_up);
        }
        // Sprint (Alt + UpArrow hoặc Alt + W)
        else if (IsKeyHeld(KeyCode.LeftAlt, KeyCode.RightAlt) && IsKeyHeld(KeyCode.UpArrow, KeyCode.W))
        {
            TriggerAnimation(sprint);
        }
        // Nhảy
        else if (Input.GetKey(KeyCode.Space))
        {
            TriggerAnimation(jump);
        }
        // Nhảy và tấn công (Space + Chuột trái)
        else if (Input.GetKey(KeyCode.Space) && Input.GetMouseButtonDown(0))
        {
            TriggerAnimation(jump_attack);
        }
        // Tấn công (Chuột trái)
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            TriggerAnimation(attack);
        }
        // Combo (Chuột phải)
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            TriggerAnimation(combo);
        }
        // Phòng thủ (Ctrl)
        else if (IsKeyHeld(KeyCode.LeftControl, KeyCode.RightControl))
        {
            TriggerAnimation(defend);
        }
        else
        {
            // Reset trạng thái tất cả các trigger
            playerAnimator.ResetTrigger(stop);
            playerAnimator.ResetTrigger(walk_up);
            playerAnimator.ResetTrigger(run_up);
            playerAnimator.ResetTrigger(sprint);
            playerAnimator.ResetTrigger(jump);
            playerAnimator.ResetTrigger(jump_attack);
            playerAnimator.ResetTrigger(attack);
            playerAnimator.ResetTrigger(combo);
            playerAnimator.ResetTrigger(defend);

            // Set trạng thái idle
            playerAnimator.SetBool(idle, true);
        }

        // Dừng lại khi thả phím
        if (IsKeyReleased(
                KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow,
                KeyCode.W, KeyCode.A, KeyCode.D, KeyCode.S,
                KeyCode.LeftShift, KeyCode.RightShift, KeyCode.LeftAlt, KeyCode.RightAlt,
                KeyCode.Space, KeyCode.LeftControl, KeyCode.RightControl,
                KeyCode.Mouse0, KeyCode.Mouse1))
        {
            isPressKey = false;
            playerAnimator.SetTrigger(stop);

            playerAnimator.ResetTrigger(walk_up);
            playerAnimator.ResetTrigger(run_up);
            playerAnimator.ResetTrigger(sprint);
            playerAnimator.ResetTrigger(jump);
            playerAnimator.ResetTrigger(jump_attack);
            playerAnimator.ResetTrigger(attack);
            playerAnimator.ResetTrigger(combo);
            playerAnimator.ResetTrigger(defend);
        }
    }

    // Hàm gọi trigger và cập nhật trạng thái
    private void TriggerAnimation(string triggerName)
    {
        if (!isPressKey)
        {
            isPressKey = true;
            playerAnimator.SetBool(idle, false);
            playerAnimator.SetTrigger(triggerName);
        }
    }
}
