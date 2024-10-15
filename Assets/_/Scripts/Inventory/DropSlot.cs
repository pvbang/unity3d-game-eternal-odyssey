using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public bool isInventorySlot;  // Đánh dấu đây là ô của Inventory hay GroundItems

    public void OnDrop(PointerEventData eventData)
    {
        // Kiểm tra nếu có item đang được kéo
        DraggableItem draggedItem = eventData.pointerDrag.GetComponent<DraggableItem>();
        if (draggedItem != null)
        {
            // Chuyển item vào ô mới
            draggedItem.transform.SetParent(transform);
            draggedItem.transform.localPosition = Vector3.zero;

            // Cập nhật Inventory hoặc GroundItems tùy vào loại slot
            if (isInventorySlot)
            {
                InventoryItemSystem.instance.AddItemToInventory(draggedItem.name);
            }
            else
            {
                InventoryItemSystem.instance.RemoveItemFromInventory(draggedItem.name);
            }
        }
    }
}
