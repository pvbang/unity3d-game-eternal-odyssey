using System.Collections.Generic;
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
    private Vector2 originalPositionAfterDrag;
    private Vector2 originalPosition;
    public Image backgroundImage;
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
        originalPositionAfterDrag = originalPosition;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        originalPositionAfterDrag = GetComponent<RectTransform>().anchoredPosition;
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
            bool isPositionChanged = originalPosition != originalPositionAfterDrag;

            if (isPositionChanged)
            {
                Debug.Log("Vị trí đã thay đổi");
            }
            else
            {
                Debug.Log("Vị trí không thay đổi");
            } 

                if (eventData.pointerEnter.name == "GroundPanel" && isPositionChanged)
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

                AddNewChest();

                Destroy(gameObject);
            }
            else if (eventData.pointerEnter.name == "InventoryPanel" || eventData.pointerEnter.CompareTag("InventorySlot") && isPositionChanged)
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

    private void AddNewChest()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;

            Vector3 chestPosition = playerPosition + player.transform.forward * 2;

            LayerMask groundLayer = LayerMask.GetMask("Ground");
            if (Physics.Raycast(chestPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, 50f, groundLayer))
            {
                chestPosition.y = hit.point.y;
                Debug.Log($"Rương sẽ được đặt tại vị trí mặt đất: {chestPosition}");
            }
            else
            {
                // Nếu không tìm thấy mặt đất, gán Y theo người chơi
                chestPosition.y = playerPosition.y;
                Debug.LogWarning("Không tìm thấy mặt đất, sử dụng chiều cao của người chơi.");
            }

            Debug.Log("Tạo Chest tại vị trí: " + chestPosition + " " + itemId + " " + quantity);

            if (ChestUIManager.Instance == null)
            {
                Debug.LogError("ChestManager.Instance là null!");
                return;
            }
            ChestUIManager.Instance.CreateNewChest(chestPosition, itemId, quantity, (success) =>
            {
                if (success)
                {
                    var chestData = new ItemRealtime
                    {
                        chestId = System.Guid.NewGuid().ToString(),
                        location = new Location(chestPosition.x, chestPosition.y, chestPosition.z),
                        items = new Dictionary<string, MaterialsUpdate>
                                {
                                 { itemId, new MaterialsUpdate(itemId, quantity) }
                                },
                        status = "Closed"
                    };
                    ChestUIManager.Instance.DisplayChest(chestData);
                    Debug.Log("Tạo Chest thành công!");
                }
                else
                {
                    Debug.LogError("Lỗi khi tạo Chest!");
                }
            });
        }
        else
        {
            Debug.LogError("Không tìm thấy Player trong scene!");
        }
    }    
}
