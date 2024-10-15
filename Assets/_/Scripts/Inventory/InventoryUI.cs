using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventoryItemSystem inventorySystem;  // Tham chiếu tới hệ thống Inventory
    public Transform inventoryPanel;  // Panel chứa các ô Inventory
    public GameObject inventorySlotPrefab;  // Prefab cho ô Inventory
    public IconManager iconManager;  // Tham chiếu tới IconManager để lấy icon

    private void Start()
    {
        if (inventorySystem != null)
        {
            // Đăng ký sự kiện cập nhật UI khi Inventory thay đổi
            inventorySystem.onInventoryUpdated += UpdateInventoryUI;
            Debug.Log("Đã đăng ký sự kiện onInventoryUpdated cho InventoryUI.");

            // Gọi cập nhật UI ngay khi bắt đầu
            UpdateInventoryUI();
        }
        else
        {
            Debug.LogError("inventorySystem chưa được gán trong InventoryUI.");
        }
    }

    public void UpdateInventoryUI()
    {
        Debug.Log("Đã gọi UpdateInventoryUI()");

        // Kiểm tra xem inventorySystem có dữ liệu không
        if (inventorySystem == null || inventorySystem.inventory == null)
        {
            Debug.LogError("inventorySystem hoặc inventory trống.");
            return;
        }

        Debug.Log($"Số lượng item trong inventory: {inventorySystem.inventory.Count}");

        // Xóa toàn bộ các Slot cũ trong Panel để cập nhật lại
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }


        // Tạo các Slot mới dựa trên danh sách Inventory hiện tại
        foreach (var playerItem in inventorySystem.inventory)
        {
            if (playerItem == null)
            {
                Debug.LogError("PlayerItem bị null!");
                continue; // Nếu item null, bỏ qua vòng lặp
            }

            Debug.Log($"Đang hiển thị item: {playerItem.itemId}, số lượng: {playerItem.quantity}");

            // Tạo một Slot mới từ Prefab
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);

            // Hiển thị số lượng của item
            var itemQuantityText = slot.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
            if (itemQuantityText != null)
            {
                itemQuantityText.text = $"x{playerItem.quantity}";
            }
            else
            {
                Debug.LogError("Không tìm thấy component TextMeshProUGUI cho ItemQuantity trong prefab.");
            }

            // Hiển thị icon của item từ IconManager
            var itemIconImage = slot.transform.Find("ItemIcon").GetComponent<Image>();
            if (itemIconImage != null)
            {
                Sprite itemIcon = iconManager.GetIcon(playerItem.itemId);  // Lấy icon từ IconManager
                if (itemIcon != null)
                {
                    itemIconImage.sprite = itemIcon;  // Gán icon cho Image
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy icon cho item {playerItem.itemId}");
                }
            }
            else
            {
                Debug.LogError("Không tìm thấy thành phần ItemIcon trong prefab.");
            }
        }
    }


}
