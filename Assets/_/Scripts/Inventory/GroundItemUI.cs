using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroundItemUI : MonoBehaviour
{
    public GroundItemSystem groundSystem;  // Tham chiếu tới GroundItemSystem
    public Transform groundPanel;  // Content của GroundItemsPanel
    public GameObject itemPrefab;  // Prefab chung cho item

    // Hàm cập nhật UI của GroundItemsPanel
    public void UpdateGroundUI(List<Inventory> groundItems)
    {
        Debug.Log("Cập nhật Ground UI");

        // Nếu không có item nào trên mặt đất, xóa hết các item trong UI
        if (groundItems == null || groundItems.Count == 0)
        {
            Debug.LogWarning("Không có vật phẩm trên mặt đất.");

            // Xóa tất cả các slot trong Ground Panel
            foreach (Transform child in groundPanel)
            {
                Destroy(child.gameObject);
            }

            return;
        }

        // Xóa các slot cũ trong Ground Panel trước khi tạo mới
        foreach (Transform child in groundPanel)
        {
            Destroy(child.gameObject);
        }

        // Tạo slot mới cho mỗi item trên mặt đất
        foreach (var groundItem in groundItems)
        {
            if (groundItem == null || groundItem.item == null)
            {
                Debug.LogWarning("Item bị null, không thể tạo slot.");
                continue;
            }

            GameObject slot = Instantiate(itemPrefab, groundPanel);

            slot.tag = "GroundSlot";
            Debug.Log($"Item: {groundItem.item.itemId} được thêm vào UI Ground với số lượng {groundItem.quantity}");

            var itemIcon = slot.transform.Find("ItemIcon").GetComponent<Image>();
            itemIcon.sprite = FindObjectOfType<IconManager>().GetIcon(groundItem.item.itemId);

            var quantityText = slot.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
            quantityText.text = $"x{groundItem.quantity}";

            var draggableItem = slot.GetComponent<DraggableItem>();
            draggableItem.inventorySystem = FindObjectOfType<InventoryItemSystem>();
            draggableItem.groundSystem = groundSystem;
            draggableItem.item = groundItem.item;
            draggableItem.itemId = groundItem.item.itemId;
            draggableItem.quantity = groundItem.quantity;

            slot.transform.SetParent(groundPanel);
        }

    }
}
