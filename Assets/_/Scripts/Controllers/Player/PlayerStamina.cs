using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    public Image staminaBar;

    private float currentStamina = 100f;
    private float maxStamina = 100f;
    private float baseStaminaDecreaseRate = 50f; // Tốc độ giảm stamina cơ bản ở Level 1
    private float staminaRecoveryRate = 5f;      // Tốc độ hồi phục stamina mỗi giây khi không chạy
    private float staminaUpdateInterval = 0.01f;  // Thời gian cập nhật stamina mỗi 0.2 giây
    private float timeSinceLastUpdate = 0f;      // Thời gian đã trôi qua từ lần cập nhật cuối

    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        // Chỉ thực hiện tính toán stamina khi đủ 0.01 giây
        if (timeSinceLastUpdate >= staminaUpdateInterval)
        {
            // Lấy level người chơi từ PlayerPrefs (có thể tùy chỉnh theo cách lưu của game)
            int playerLevel = PlayerPrefs.GetInt(ConstantManager.PLAYER_LEVEL, 1);

            // Tính stamina decrease rate theo cấp độ
            float staminaDecreaseRate = baseStaminaDecreaseRate * Mathf.Pow(0.9f, playerLevel - 1);

            // Kiểm tra các trạng thái di chuyển
            bool isMoving = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
                            Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
                            Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
            bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            if (isMoving && isShiftHeld && currentStamina > 0)
            {
                // Giảm stamina theo cấp độ khi chạy
                currentStamina -= staminaDecreaseRate * staminaUpdateInterval;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Đảm bảo không dưới 0
            }
            else
            {
                // Hồi phục stamina dần dần khi không chạy
                currentStamina += staminaRecoveryRate * staminaUpdateInterval;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); // Đảm bảo không vượt quá maxStamina
            }

            PlayerPrefs.SetFloat(ConstantManager.PLAYER_STAMINA, currentStamina);

            if (currentStamina != maxStamina)
            {
                // Kiểm tra và cập nhật UI nếu cần thiết
                UpdateStaminaUI();
            }

            // Reset thời gian sau khi cập nhật
            timeSinceLastUpdate = 0f;
        }
    }

    void UpdateStaminaUI()
    {
        staminaBar.fillAmount = currentStamina / maxStamina;
    }
}
