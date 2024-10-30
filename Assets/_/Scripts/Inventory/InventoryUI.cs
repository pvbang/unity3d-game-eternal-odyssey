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
    Color colorDefault = new Color(248f / 255f, 248f / 255f, 248f / 255f, 81f / 255f); 
    public TMP_InputField quantityInput;  // Ô để hiển thị số lượng
    public Slider quantitySlider;         // Slider điều chỉnh số lượng
    public Button splitButton;            // Nút để tách item
    private Inventory selectedItem;       // Lưu trữ item được chọn hiện tại
    private GameObject currentlySelectedSlot = null;
    private Inventory selectedInventoryItem = null;

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
        splitButton.onClick.AddListener(SplitItem); // Thêm sự kiện nhấn nút tách
        quantitySlider.onValueChanged.AddListener(UpdateQuantityFromSlider); // Thêm sự kiện khi slider thay đổi
    }

    private void UpdateQuantityFromSlider(float value)
    {
        quantityInput.text = Mathf.FloorToInt(value).ToString(); // Cập nhật ô số lượng theo slider
    }

    private void SplitItem()
    {
        if (selectedItem == null) return;

        int splitQuantity = int.Parse(quantityInput.text);
        if (splitQuantity <= 0 || splitQuantity >= selectedItem.quantity) return;

        selectedItem.quantity -= splitQuantity;

        Inventory newItem = new Inventory(selectedItem.item, splitQuantity);
        inventorySystem.inventory.Add(newItem);
        inventorySystem.onInventoryUpdated?.Invoke();  // Cập nhật lại UI

        // Reset lựa chọn
        selectedItem = null;
        quantityInput.text = "";
        quantitySlider.value = 0;
    }

    private IEnumerator WaitForDataAndDisplayInventory()
    {
        List<Inventory> inventoryItem = new List<Inventory>();

        yield return new WaitUntil(() => inventorySystem.isDataLoaded);

        Debug.Log("Đã vào đây");
        inventorySystem.LoadInventoryFromFirebase(inventoryList =>
        {
            Debug.Log("Đã vào đây 1");
            List<Inventory> inventoryListItem = new List<Inventory>();
            foreach (var item in inventoryList)
            {
                Debug.Log("Đã vào đây 2");
                if (inventorySystem.itemDetails.TryGetValue(item.itemId, out Item itemDetail))
                {
                    Inventory completeInventory = new Inventory(itemDetail, item.quantity);
                    Debug.Log($"Thêm item vào inventory: {completeInventory.item.itemId} x{completeInventory.quantity}");
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


        foreach (var playerItem in inventory)
        {
            if (playerItem == null) continue;
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel);
            slot.tag = "InventorySlot";
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
            button.onClick.AddListener(() => OnClickItem(slot, playerItem));
            slot.transform.SetParent(inventoryPanel);
        }
    }

    private void OnClickItem(GameObject slot, Inventory item)
    {
        // Ẩn background đã chọn trước đó
        if (currentlySelectedSlot != null)
        {
            var previousSelected = currentlySelectedSlot.transform.Find("Selected");
            if (previousSelected != null)
            {
                previousSelected.gameObject.SetActive(false);
            }
        }

        // Hiển thị background của item hiện tại
        var selectedBackground = slot.transform.Find("Selected");
        if (selectedBackground != null)
        {
            selectedBackground.gameObject.SetActive(true);
        }

        if (itemInfoDisplay != null)
        {
            itemInfoDisplay.ShowItemInfo(item.item);
        }

        // Cập nhật số lượng cho Input và Slider
        quantityInput.text = item.quantity.ToString();
        quantitySlider.maxValue = item.quantity;
        quantitySlider.value = item.quantity;

        selectedInventoryItem = item;
        currentlySelectedSlot = slot;
    }
}