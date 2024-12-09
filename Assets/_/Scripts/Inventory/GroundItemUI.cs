using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroundItemUI : MonoBehaviour
{
    public GroundItemSystem groundSystem;
    public Transform groundPanel;
    public GameObject itemPrefab;

    private void Start()
    {
        if (groundSystem == null)
        {
            groundSystem = FindObjectOfType<GroundItemSystem>();
            if (groundSystem == null)
            {
                Debug.LogError("Không tìm thấy GroundItemSystem trong scene.");
            }
        }
    }

    public void UpdateGroundUI(List<Inventory> groundItems)
    {
        foreach (Transform child in groundPanel)
        {
            Destroy(child.gameObject);
        }

        if(groundItems == null || groundItems.Count == 0)
        {
            Debug.Log("Không có item nào trên mặt đất.");
            return;
        }

        foreach (var groundItem in groundItems)
        {
            if (groundItem == null || groundItem.item == null)
            {
                Debug.LogWarning("Item bị null, không thể tạo slot.");
                continue;
            }

            GameObject slot = Instantiate(itemPrefab, groundPanel);

            slot.tag = "GroundSlot";

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

    public void DisplayItemsFromChest(string chestId)
    {
        Dictionary<string, ItemRealtime> listChests = ChestUIManager.Instance.ListChests;
        if (listChests.ContainsKey(chestId))
        {
            ItemRealtime itemRealtime = listChests[chestId];
            if (itemRealtime != null && itemRealtime.items.Count > 0)
            {
                foreach (var itemEntry in itemRealtime.items)
                {
                    string itemId = itemEntry.Key;
                    MaterialsUpdate itemData = itemEntry.Value;
                    if (groundSystem != null && FindObjectOfType<InventoryItemSystem>() != null)
                    {
                        Item itemDetail = FindObjectOfType<InventoryItemSystem>().GetItemById(itemId);
                        if (itemDetail != null)
                        {
                            groundSystem.AddItemToGround(itemDetail, itemData.quantity);
                        }
                        else
                        {
                            Debug.LogWarning($"Không tìm thấy thông tin chi tiết cho item với ID: {itemId}");
                        }
                    }
                }
                UpdateGroundUI(groundSystem.GetGroundItems());
            }
            else
            {
                Debug.Log("Rương trống hoặc không tìm thấy rương.");
            }
        }
    }

    public void OnChestOpened(string chestId)
    {
        DisplayItemsFromChest(chestId);
    }
}
