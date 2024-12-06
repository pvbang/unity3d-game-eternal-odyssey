using UnityEngine;
using UnityEngine.EventSystems;

public class CursorManager : MonoBehaviour
{
    private void Start()
    {
        // Bật con trỏ chuột và mở khóa nó
        ShowCursor();
    }

    private void Update()
    {
        ShowCursor();
    }


    // Hàm để hiển thị con trỏ chuột
    public void ShowCursor()
    {
        Cursor.visible = true;  // Hiển thị con trỏ
        Cursor.lockState = CursorLockMode.None;  // Không khóa con trỏ, cho phép di chuyển tự do
    }

    // Hàm để ẩn con trỏ chuột (nếu cần)
    public void HideCursor()
    {
        Cursor.visible = false;  // Ẩn con trỏ
        Cursor.lockState = CursorLockMode.Locked;  // Khóa con trỏ vào giữa màn hình
    }
}
