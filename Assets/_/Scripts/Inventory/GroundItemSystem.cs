using NullSave.TOCK.Inventory;
using System.Collections.Generic;
using UnityEngine;

public class GroundItemSystem : MonoBehaviour
{
    private Dictionary<string, Inventory> groundItems = new Dictionary<string, Inventory>(); // Dùng Dictionary cho hiệu quả hơn


    public void AddItemToGround(Item item, int quantity)
    {
        if (item == null || string.IsNullOrEmpty(item.itemId))
        {
            Debug.Log("Item hoặc itemId không hợp lệ");
            return;
        }

        if (groundItems.ContainsKey(item.itemId))
        {
            groundItems[item.itemId].quantity += quantity;  // Cộng dồn số lượng
        }
        else
        {
            groundItems[item.itemId] = new Inventory(item, quantity);  // Thêm item mới với thông tin đầy đủ
        }

        Debug.Log($"Đã thêm item {item.itemId} vào Ground với số lượng {quantity}");
    }


    public void RemoveItemFromGround(string itemId, int quantity)
    {
        if (groundItems.ContainsKey(itemId))
        {
            groundItems[itemId].quantity -= quantity;
            if (groundItems[itemId].quantity <= 0)
            {
                groundItems.Remove(itemId);  // Xóa item khi số lượng <= 0
            }
        }
    }

    public List<Inventory> GetGroundItems()
    {
        return new List<Inventory>(groundItems.Values);
    }
}
