using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public TMP_InputField quantityInput;
    public Slider quantitySlider;
    public Button splitButton;
    private Inventory selectedItem;
    private GameObject currentlySelectedSlot = null;
    public Button usedItemButton;
    private List<ChestInteractionHandler> chestInteractionHandlers;
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
        StartCoroutine(WaitForDataAndDisplayInventory());
        splitButton.onClick.AddListener(SplitItem);
        usedItemButton.onClick.AddListener(OnUseItem);
        quantitySlider.onValueChanged.AddListener(UpdateQuantityFromSlider);
        chestInteractionHandlers = new List<ChestInteractionHandler>(FindObjectsOfType<ChestInteractionHandler>());
    }

    private void UpdateQuantityFromSlider(float value)
    {
        quantityInput.text = Mathf.FloorToInt(value).ToString();

    }

    private void SplitItem()
    {
        if (selectedItem == null) return;
        int splitQuantity = int.Parse(quantityInput.text);
        if (splitQuantity <= 0 || splitQuantity >= selectedItem.quantity) return;

        selectedItem.quantity -= splitQuantity;
        Debug.Log($"Check quantity item {splitQuantity} {selectedItem.item.name}");
        Inventory newItem = new Inventory(selectedItem.item, splitQuantity);
        inventorySystem.inventory.Add(newItem);
        UpdateInventoryUI(inventorySystem.inventory);
        selectedItem = null;
        quantityInput.text = "";
        quantitySlider.value = 0;
    }

    private IEnumerator WaitForDataAndDisplayInventory()
    {
        List<Inventory> inventoryItem = new List<Inventory>();

        yield return new WaitUntil(() => inventorySystem.isDataLoaded);

        inventorySystem.LoadInventoryFromFirebase(inventoryList =>
        {
            Debug.Log("Data from Inventory UI:"+ inventoryList.Count);
            List<Inventory> inventoryListItem = new List<Inventory>();
            foreach (var item in inventoryList)
            {
                if (inventorySystem.itemDetails.TryGetValue(item.itemId, out Item itemDetail))
                {
                    Inventory completeInventory = new Inventory(itemDetail, item.quantity);
                    inventoryListItem.Add(completeInventory);
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy thông tin chi tiết cho item với ID: {item.itemId}");
                }
            }
            inventoryItem = inventoryListItem;
        });

        float timeout = 30f;
        float elapsedTime = 0f;

        while (inventoryItem.Count == 0 && elapsedTime < timeout)
        {
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }

        if (inventoryItem == null || inventoryItem.Count == 0)
        {
            Debug.LogWarning("Không có dữ liệu inventory từ Firebase sau khi hết thời gian chờ.");
            yield break;
        }

        totalItem.text = inventoryItem.Count.ToString();
        if (inventoryItem != null && inventoryItem.Count > 0)
        {
            UpdateInventoryUI(inventoryItem);
        }
    }
    public void UpdateInventoryUI(List<Inventory> inventory)
    {
        if (inventory == null || inventory.Count == 0)
        {
            Debug.LogWarning("Inventory trống hoặc không có dữ liệu.");
            return;
        }

        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (var playerItem in inventory)
        {
            if (playerItem == null) continue;
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
            slot.tag = "InventorySlot";
            var itemIcon = slot.transform.Find("ItemIcon").GetComponent<Image>();
            itemIcon.sprite = iconManager.GetIcon(playerItem.item.itemId);
            var quantityText = slot.transform.Find("ItemQuantity").GetComponent<TextMeshProUGUI>();
            quantityText.text = $"x{playerItem.quantity}";
            var draggableItem = slot.GetComponent<DraggableItem>();
            draggableItem.inventorySystem = FindObjectOfType<InventoryItemSystem>();
            draggableItem.groundSystem = groundSystem;
            draggableItem.item = playerItem.item;
            draggableItem.itemId = playerItem.item.itemId;
            draggableItem.quantity = playerItem.quantity;
            Button button = slot.AddComponent<Button>();
            button.onClick.AddListener(() => OnClickItem(slot, playerItem));
            slot.transform.SetParent(inventoryPanel);
        }
    }

    public void UpdateButtonVisibility(ItemProperties itemType)
    {
        if (itemType == ItemProperties.Buff)
        {
            usedItemButton.gameObject.SetActive(true);
        }
        else
        {
            usedItemButton.gameObject.SetActive(false);
        }
    }

    private void OnClickItem(GameObject slot, Inventory item)
    {
        if (currentlySelectedSlot != null)
        {
            var previousSelected = currentlySelectedSlot.transform.Find("BackgroundSelected");
            if (previousSelected != null)
            {
                previousSelected.gameObject.SetActive(false);
            }
        }
        UpdateButtonVisibility(item.item.properties);

        // Hiển thị background của item hiện tại
        var selectedBackground = slot.transform.Find("BackgroundSelected");
        if (selectedBackground != null)
        {
            selectedBackground.gameObject.SetActive(true);
        }

        // Cập nhật thông tin chi tiết của item nếu có `itemInfoDisplay`
        if (itemInfoDisplay != null)
        {
            itemInfoDisplay.ShowItemInfo(item.item);
        }

        // Cập nhật số lượng cho Input và Slider
        quantityInput.text = item.quantity.ToString();
        quantitySlider.maxValue = item.quantity;
        quantitySlider.value = item.quantity;

        // Lưu trạng thái item hiện tại
        currentlySelectedSlot = slot;
        selectedItem = item;
    }

    public void OnUseItem()
    {
        if (selectedItem == null) return;

        string itemId = selectedItem.item.itemId;
        int value = 0;
        float valueSpeed = 0;
        int duration = selectedItem.item.usingTime;

        switch (selectedItem.item.itemId)
        {
            case "buff_hp":
                value = selectedItem.item.attributes.health.current;
                break;
            case "buff_speed":
                valueSpeed = selectedItem.item.attributes.runSpeed;
                break;
            case "buff_dame":
                value = selectedItem.item.attributes.damage.from;
                break;
            case "buff_resistance":
                value = selectedItem.item.attributes.resistance;
                break;
            case "buff_crit":
                value = selectedItem.item.attributes.resistance;
                break;
            default:
                Console.WriteLine("Invalid item ID");
                break;
        }

        UseItems useItems = new UseItems();
        bool usedSuccessfully = false;

        switch (itemId)
        {
            case "buff_hp":
                if (useItems != null)
                {
                    useItems.Use(itemId, value, valueSpeed, duration);
                    usedSuccessfully = true;
                }
                else
                {
                    usedSuccessfully = false;
                }
                break;
            default:
                useItems?.Use(itemId, value, valueSpeed, duration);
                usedSuccessfully = true; // Giả sử các loại buff khác luôn thành công
                break;
        }
        // Giảm quantity và cập nhật Inventory
        if (usedSuccessfully)
        {
            selectedItem.quantity--;
            if (selectedItem.quantity <= 0)
            {
                inventorySystem.inventory.Remove(selectedItem);
                selectedItem = null;
            }

            Debug.Log($"Số lượng item sau khi sử dụng: {selectedItem?.quantity ?? 0}, {itemId}");
            ManagerInventory.UpdateItemQuantity(itemId, selectedItem?.quantity ?? 0, inventorySystem.worldId, inventorySystem.userId, (success) =>
            {
                if (success)
                {
                    Debug.Log("Cập nhật item sau khi đã sử dụng thành công!");
                }
                else
                {
                    Debug.Log("Cập nhật thất bại.");
                }
            });

            // Cập nhật lại UI
            inventorySystem.onInventoryUpdated?.Invoke();
            InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
            if (inventoryUI != null)
            {
                inventoryUI.UpdateInventoryUI(inventorySystem.inventory);
            }
        }
        else
        {
            Debug.Log("Item không được sử dụng vì điều kiện không phù hợp.");
        }
    }
}