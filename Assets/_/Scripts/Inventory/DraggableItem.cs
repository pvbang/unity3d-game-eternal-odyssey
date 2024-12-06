using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventoryItemSystem inventorySystem;
    public GroundItemSystem groundSystem;

    private Transform originalParent;
    private CanvasGroup canvasGroup;

    public Item item;
    public string itemId;
    public int quantity;
    private Vector2 originalPosition;

    public Image backgroundImage;  // Background của item

    public Sprite normalBackgroundSprite;
    public Sprite rareBackgroundSprite;
    public Sprite legendaryBackgroundSprite;
    Color colorDefault = new Color(0f, 0f, 0f, 163f / 255f); 
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        // Kiểm tra loại item và gán màu tương ứng
        switch (item.type)
        {
            case ItemType.Normal:
                backgroundImage.color = colorDefault;
                break;
            case ItemType.Rare:
                backgroundImage.sprite = rareBackgroundSprite;
                break;
            case ItemType.Legendary:
                backgroundImage.sprite = legendaryBackgroundSprite;
                break;
            default:
                backgroundImage.color = colorDefault;
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;  // Lưu parent hiện tại
        originalPosition = GetComponent<RectTransform>().anchoredPosition; // Lưu vị trí gốc
        transform.SetParent(transform.root);  // Đưa item lên trên cùng UI để dễ kéo thả
        transform.SetAsLastSibling();  // Đảm bảo item ở trên cùng của hierarchy
        canvasGroup.blocksRaycasts = false;  // Tắt raycast để có thể kéo qua các UI khác
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;  // Cập nhật vị trí theo con trỏ
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter != null)
        {
            if (inventorySystem == null)
            {
                Debug.Log("inventorySystem chưa được gán");
                return;
            }

            if (groundSystem == null)
            {
                Debug.Log("groundSystem chưa được gán");
                return;
            }
            Debug.Log($"Item được thả vào: {eventData.pointerEnter.name}");

            // Thả vào GroundPanel
            if (eventData.pointerEnter.name == "GroundPanel")
            {
                inventorySystem.RemoveItemFromInventory(itemId, quantity);
                groundSystem.AddItemToGround(item, quantity);

                GroundItemUI groundUI = FindObjectOfType<GroundItemUI>();
                InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
                inventoryUI.UpdateInventoryUI(inventorySystem.inventory);

                ManagerInventory.DeleteItem(itemId, quantity, inventorySystem.worldId, inventorySystem.userId, (success, message) =>
                {
                    if (success)
                    {
                        Debug.Log(message);
                        // Thực hiện các thao tác tiếp theo nếu cần
                    }
                    else
                    {
                        Debug.LogError(message);
                    }
                });
                if (groundUI != null)
                {
                    groundUI.UpdateGroundUI(groundSystem.GetGroundItems());
                }
                else
                {
                    Debug.LogError("Không tìm thấy GroundItemUI trong scene");
                }

                Destroy(gameObject);
            }
            // Thả vào InventoryPanel hoặc ô trong InventoryPanel
            else if (eventData.pointerEnter.name == "InventoryPanel" || eventData.pointerEnter.CompareTag("InventorySlot"))
            {
                inventorySystem.inventory.Add(new Inventory(item, quantity));
                groundSystem.RemoveItemFromGround(itemId, quantity);

                ManagerInventory.AddItem(itemId, quantity, inventorySystem.worldId, inventorySystem.userId, (success, message) =>
                {
                    if (success)
                    {
                        Debug.Log(message);
                        // Thực hiện các hành động khác khi thêm thành công
                    }
                    else
                    {
                        Debug.LogError(message);
                    }
                }); InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
                if (inventoryUI != null)
                {
                    inventoryUI.UpdateInventoryUI(inventorySystem.inventory);
                }
                else
                {
                    Debug.LogError("Không tìm thấy InventoryUI trong scene");
                }

                Destroy(gameObject);
            }
            // Trả item về vị trí gốc khi thả vào vùng không hợp lệ
            else
            {
                transform.SetParent(originalParent);
                GetComponent<RectTransform>().anchoredPosition = originalPosition;
            }
        }
        else
        {
            // Không có đối tượng nào được trỏ vào
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = originalPosition;
        }
    }
}
