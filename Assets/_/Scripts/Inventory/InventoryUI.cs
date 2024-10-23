using NullSave.TOCK.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    public InventoryItemSystem inventorySystem;
    public GroundItemSystem groundSystem;
    public Transform inventoryPanel;
    public GameObject inventorySlotPrefab;
    public IconManager iconManager;
    private List<Inventory> inventoryItem;
    public ItemInfoDisplay itemInfoDisplay;
    private void Start()
    {
        if (inventorySystem == null)
        {
            inventorySystem = FindObjectOfType<InventoryItemSystem>();

            if (inventorySystem == null)
            {
                Debug.LogError("Không tìm thấy InventoryItemSystem trong scene.");
                return;
            }
        }
        inventorySystem.onInventoryUpdated += () => UpdateInventoryUI(inventoryItem);

        // Chạy coroutine để đợi đến khi dữ liệu được tải
        StartCoroutine(WaitForDataAndDisplayInventory());
    }

    private IEnumerator WaitForDataAndDisplayInventory()
    {
        inventoryItem = new List<Inventory>(); 

        yield return new WaitUntil(() => inventorySystem.isDataLoaded);

        InventoryItemSystem.LoadInventoryFromFirebase(inventoryItem_ => inventoryItem = inventoryItem_);

        float timeout = 30f;
        float elapsedTime = 0f;

        while (inventoryItem.Count == 0 && elapsedTime < timeout)
        {
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        if (inventoryItem.Count == 0)
        {
            Debug.LogWarning("Không có dữ liệu inventory từ Firebase sau khi hết thời gian chờ.");
            yield break;
        }

        // Khi dữ liệu đã được tải, cập nhật Inventory UI
        UpdateInventoryUI(inventoryItem);
    }
    public void UpdateInventoryUI(List<Inventory> inventory)
    {
        Debug.Log("Cập nhật Inventory UI");

        if (inventory == null || inventory.Count == 0)
        {
            Debug.LogWarning("Inventory trống hoặc không có dữ liệu.");
            return;
        }

        // Xóa các slot cũ trong Inventory Panel
        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        // Tạo slot mới cho mỗi item trong Inventory
        foreach (var playerItem in inventory)
        {
            if (playerItem == null) continue;

            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);

            // Gán tag cho slot để nhận diện
            slot.tag = "InventorySlot";

            // Cập nhật icon và số lượng
            var itemIcon = slot.transform.Find("ItemIcon").GetComponent<Image>();
            itemIcon.sprite = iconManager.GetIcon(playerItem.item.itemId);

            var quantityText = slot.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
            quantityText.text = $"x{playerItem.quantity}";

            // Gán DraggableItem để cho phép kéo thả
            var draggableItem = slot.GetComponent<DraggableItem>();
            draggableItem.inventorySystem = FindObjectOfType<InventoryItemSystem>();
            draggableItem.groundSystem = groundSystem;
            draggableItem.item = playerItem.item;
            draggableItem.itemId = playerItem.item.itemId;
            draggableItem.quantity = playerItem.quantity;

            Button button = slot.AddComponent<Button>();
            button.onClick.AddListener(() => OnClickItem(playerItem.item));

            slot.transform.SetParent(inventoryPanel);
        }
    }

    public void OnClickItem(Item item)
    {
        if (itemInfoDisplay != null)
        {
            itemInfoDisplay.ShowItemInfo(item);
        }
        else
        {
            Debug.LogError("itemInfoDisplay chưa được gán.");
            return;
        }
    }
}