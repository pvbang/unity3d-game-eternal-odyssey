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

            if (eventData.pointerEnter.name == "GroundPanel")
            {
                Debug.Log($"Check {item.itemId} item id: {itemId}");
                // Xóa item khỏi Inventory và thêm vào Ground
                inventorySystem.RemoveItemFromInventory(itemId);
                groundSystem.AddItemToGround(item, quantity);

              
                // Cập nhật UI Ground và xóa item khỏi Inventory UI
                GroundItemUI groundUI = FindObjectOfType<GroundItemUI>();
                InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
                inventoryUI.UpdateInventoryUI(inventorySystem.inventory);

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
            else if (eventData.pointerEnter.name == "InventoryPanel")
            {
                // Xóa item khỏi Ground và thêm lại vào Inventory
                groundSystem.RemoveItemFromGround(itemId, quantity);
                inventorySystem.AddItemToInventory(itemId);

                Transform inventoryContent = GameObject.Find("InventoryContent").transform;
                transform.SetParent(inventoryContent);
                transform.localPosition = Vector3.zero; 

                // Cập nhật UI Inventory
                InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
                if (inventoryUI != null)
                {
                    inventoryUI.UpdateInventoryUI(inventorySystem.inventory);
                }
                else
                {
                    Debug.LogError("Không tìm thấy InventoryUI trong scene");
                }
            }
            else
            {
                Debug.Log("Không thả vào gì cả, trả item về vị trí cũ");
                // Không thả vào đâu cả, trả về vị trí và parent gốc
                transform.SetParent(originalParent);
                GetComponent<RectTransform>().anchoredPosition = originalPosition;
            }
        }
        else
        {
            Debug.Log("Không có đối tượng nào được trỏ vào");
            // Không có đối tượng được trỏ vào, trả item về vị trí và parent gốc
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = originalPosition;
        }
    }

}
