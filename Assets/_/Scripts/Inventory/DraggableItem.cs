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
        originalParent = transform.parent;
        originalPosition = GetComponent<RectTransform>().anchoredPosition;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
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

            if (eventData.pointerEnter.name == "GroundPanel")
            {
                inventorySystem.RemoveItemFromInventory(itemId, quantity);
                groundSystem.AddItemToGround(item, quantity);

                Vector3 dropPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));

                GroundItemUI groundUI = FindObjectOfType<GroundItemUI>();
                InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
                inventoryUI.UpdateInventoryUI(inventorySystem.inventory);

                ManagerInventory.DeleteItem(itemId, quantity, inventorySystem.worldId, inventorySystem.userId, (success, message) =>
                {
                    if (success)
                    {
                        Debug.Log(message);
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
            else if (eventData.pointerEnter.name == "InventoryPanel" || eventData.pointerEnter.CompareTag("InventorySlot"))
            {
                inventorySystem.inventory.Add(new Inventory(item, quantity));
                groundSystem.RemoveItemFromGround(itemId, quantity);

                ManagerInventory.AddItem(itemId, quantity, inventorySystem.worldId, inventorySystem.userId, (success, message) =>
                {
                    if (success)
                    {
                        Debug.Log(message);
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
            else
            {
                transform.SetParent(originalParent);
                GetComponent<RectTransform>().anchoredPosition = originalPosition;
            }
        }
        else
        {
            transform.SetParent(originalParent);
            GetComponent<RectTransform>().anchoredPosition = originalPosition;
        }
    }
}
