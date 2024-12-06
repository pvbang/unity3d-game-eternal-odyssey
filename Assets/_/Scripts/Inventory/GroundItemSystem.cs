using NullSave.TOCK.Inventory;
using System.Collections.Generic;
using UnityEngine;

public class GroundItemSystem : MonoBehaviour
{
    private Dictionary<string, Inventory> groundItems = new Dictionary<string, Inventory>();

    public void AddItemToGround(Item item, int quantity)
    {
        if (item == null || string.IsNullOrEmpty(item.itemId))
        {
            Debug.Log("Item hoặc itemId không hợp lệ");
            return;
        }

        if (groundItems.ContainsKey(item.itemId))
        {
            groundItems[item.itemId].quantity += quantity;
        }
        else
        {
            groundItems[item.itemId] = new Inventory(item, quantity); 
        }
    }

    public void RemoveItemFromGround(string itemId, int quantity)
    {
        if (groundItems.ContainsKey(itemId))
        {
            groundItems[itemId].quantity -= quantity;
            if (groundItems[itemId].quantity <= 0)
            {
                groundItems.Remove(itemId); 
            }
        }
    }
    public List<Inventory> GetGroundItems()
    {
        Debug.Log($"Số item trên mặt đất: {groundItems.Count}");
        return new List<Inventory>(groundItems.Values);
    }

    public void ClearAllItems()
    {
        groundItems.Clear();
        Debug.Log($"Đã xóa tất cả vật phẩm trên mặt đất. Số item còn lại {groundItems.Count}");
    }
}
