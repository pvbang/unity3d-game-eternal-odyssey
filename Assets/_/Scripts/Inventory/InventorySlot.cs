using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public string itemId;  // ID của vật phẩm trong ô này

    // Hàm thiết lập dữ liệu item cho ô
    public void SetData(string id)
    {
        itemId = id;
    }
}
