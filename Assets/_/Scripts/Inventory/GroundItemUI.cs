using TMPro;
using UnityEngine;

public class GroundItemUI : MonoBehaviour
{
    public InventoryItemSystem inventorySystem;  // Tham chiếu đến InventoryItemSystem
    public Transform groundItemsPanel;  // Panel chứa các ô vật phẩm trên mặt đất
    public GameObject groundItemSlotPrefab;  // Prefab cho các ô vật phẩm trên mặt đất

    private void Start()
    {
        // Đăng ký delegate cập nhật UI khi danh sách GroundItems thay đổi
        inventorySystem.onInventoryUpdated += UpdateGroundItemsUI;
        UpdateGroundItemsUI();  // Gọi cập nhật UI ngay khi khởi động
    }

    // Hàm cập nhật giao diện UI cho các vật phẩm trên mặt đất
    public void UpdateGroundItemsUI()
    {
        // Xóa các Slot cũ trước khi tạo lại
        foreach (Transform child in groundItemsPanel)
        {
            Destroy(child.gameObject);
        }

        // Tạo các Slot mới dựa trên danh sách các vật phẩm trên mặt đất
        foreach (var groundItem in inventorySystem.groundItems)
        {
            // Tạo một Slot mới từ Prefab
            GameObject slot = Instantiate(groundItemSlotPrefab, groundItemsPanel);

            // Gán tên là ID của item để dễ theo dõi
            slot.name = groundItem.itemId;

            // Cập nhật thông tin hiển thị cho Slot (hiển thị ID và số lượng item)
            slot.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = groundItem.itemId;  // Hiển thị ID của item
            slot.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>().text = $"x{groundItem.quantity}";  // Hiển thị số lượng

            // Kiểm tra nếu đã có thông tin chi tiết về item
            if (inventorySystem.itemDetails.ContainsKey(groundItem.itemId))
            {
                Item itemData = inventorySystem.itemDetails[groundItem.itemId];
                slot.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = itemData.name;  // Hiển thị tên của item
            }

            // Gán script DraggableItem để cho phép kéo thả item
            DraggableItem draggable = slot.GetComponent<DraggableItem>();
            if (draggable != null)
            {
                draggable.itemId = groundItem.itemId;  // Gán ID của item cho script Draggable
            }
        }
    }
}
