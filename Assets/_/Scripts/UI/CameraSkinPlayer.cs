using UnityEngine;
using UnityEngine.EventSystems; // Thêm thư viện để kiểm tra UI

public class CameraSkinPlayer : MonoBehaviour
{
    public Transform target; // Đối tượng mà camera sẽ xoay quanh (thường là nhân vật)
    public float distance = 2.3f; // Khoảng cách từ camera tới đối tượng
    public float sensitivityX = 3.0f; // Độ nhạy khi kéo chuột theo chiều ngang
    public float sensitivityY = 3.0f; // Độ nhạy khi kéo chuột theo chiều dọc (nếu cần)
    public Vector2 rotationLimitY = new Vector2(0, 90); // Giới hạn góc xoay theo trục Y (nếu cần)

    private float currentX = 0.0f; // Giá trị xoay hiện tại theo trục X
    private float currentY = 0.0f; // Giá trị xoay hiện tại theo trục Y

    // Thêm góc xoay khởi đầu
    public float initialX = 0.0f; // Góc xoay khởi đầu theo trục X
    public float initialY = 0.0f; // Góc xoay khởi đầu theo trục Y

    void Start()
    {
        if (!target)
        {
            Debug.LogWarning("Chưa gán đối tượng nhân vật cho Camera");
            return;
        }

        // Set góc xoay khởi đầu
        currentX = initialX;
        currentY = initialY;

        // Thiết lập vị trí khởi đầu của camera dựa trên góc xoay ban đầu
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position;

        // Đặt vị trí và góc quay của camera
        transform.rotation = rotation;
        transform.position = position;

        // Đảm bảo con trỏ chuột không bị khóa
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LateUpdate()
    {
        // Kiểm tra nếu chuột đang trên UI, thì không thực hiện xoay camera
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButton(0)) // Khi nhấn giữ chuột trái
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX; // Xoay camera theo trục X
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY; // Xoay camera theo trục Y (nếu cần)

            // Giới hạn góc xoay theo trục Y
            currentY = Mathf.Clamp(currentY, rotationLimitY.x, rotationLimitY.y);
        }

        // Tính toán vị trí mới của camera khi có thay đổi góc xoay
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 position = rotation * new Vector3(0, 0, -distance) + target.position;

        transform.rotation = rotation; // Xoay camera
        transform.position = position; // Di chuyển camera theo đối tượng
    }
}
