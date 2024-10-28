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
    public TextMeshProUGUI totalItem;
    public GameObject emptySlotPrefab;  // Prefab cho slot trống

    private int maxSlots = 35;

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

        inventorySystem.LoadInventoryFromFirebase(inventoryItem_ => inventoryItem = inventoryItem_);

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

        totalItem.text = inventoryItem.Count.ToString();
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

        int itemCount = inventory.Count;

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);

            if (i < inventory.Count && inventory[i] != null)
            {
                var playerItem = inventory[i];

                // Hiển thị item trong slot
                var itemIcon = slot.transform.Find("ItemIcon").GetComponent<Image>();
                itemIcon.sprite = iconManager.GetIcon(playerItem.item.itemId);
                itemIcon.enabled = true;  // Hiển thị icon item

                var quantityText = slot.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
                quantityText.text = $"x{playerItem.quantity}";

                // Gán DraggableItem để cho phép kéo thả
                var draggableItem = slot.GetComponent<DraggableItem>();
                draggableItem.inventorySystem = inventorySystem;
                draggableItem.groundSystem = groundSystem;
                draggableItem.item = playerItem.item;
                draggableItem.itemId = playerItem.item.itemId;
                draggableItem.quantity = playerItem.quantity;

                // Thêm sự kiện click cho item
                Button button = slot.AddComponent<Button>();
                button.onClick.AddListener(() => OnClickItem(playerItem.item));
            }
            else
            {
                // Đối với các slot trống, không hiển thị icon và quantity
                var itemIcon = slot.transform.Find("ItemIcon").GetComponent<Image>();
                itemIcon.enabled = false;  // Tắt icon cho các ô trống

                var quantityText = slot.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
                quantityText.enabled = false;  // Tắt hiển thị số lượng
            }

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