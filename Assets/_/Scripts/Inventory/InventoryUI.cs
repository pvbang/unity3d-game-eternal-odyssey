using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventoryItemSystem inventorySystem;  // Tham chiếu tới InventorySystem
    public TextMeshProUGUI inventoryText;  // UI Text để hiển thị thông tin Inventory

    void Start()
    {
        // Đăng ký delegate cập nhật UI khi Inventory thay đổi
        inventorySystem.onInventoryUpdated += UpdateInventoryUI;
        UpdateInventoryUI();  // Gọi cập nhật ngay lúc đầu
    }

    // Hàm cập nhật UI
    public void UpdateInventoryUI()
    {
        inventoryText.text = "";  // Reset text trước khi cập nhật
        foreach (var playerItem in inventorySystem.inventory)
        {
            // Lấy chi tiết item từ InventorySystem
            if (inventorySystem.itemDetails.ContainsKey(playerItem.itemId))
            {
                Item itemData = inventorySystem.itemDetails[playerItem.itemId];
                inventoryText.text += $"{itemData.name} (x{playerItem.quantity})\n";
            }
            else
            {
                inventoryText.text += $"Loading item: {playerItem.itemId}...\n";
            }
        }
    }
}
