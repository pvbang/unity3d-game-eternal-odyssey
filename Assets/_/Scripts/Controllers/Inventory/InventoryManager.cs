using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static IEnumerator DeleteItem(string itemId, int quantity, string worldId, string userId)
    {
        var inventoryRef = FirebaseManager.instance.databaseReference.
            Child("worlds").
            Child(worldId).
            Child("playerWorld").
            Child(worldId).
            Child(userId).Child("inventorys");
      
        var task = inventoryRef.GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi lấy thông tin inventory: " + userId);
            yield break;
        }

        if (task.Result.Exists)
        {
            foreach (var item in task.Result.Children)
            {
                string currentItemId = item.Key;
                var itemData = item.Child("itemId").Value.ToString();

                if(itemData == itemId)
                {
                    int currentQuantity = int.Parse(item.Child("quantity").Value.ToString());

                    if(currentQuantity == quantity)
                    {
                        var deleteTask = inventoryRef.Child(currentItemId).RemoveValueAsync();
                        yield return new WaitUntil(() => deleteTask.IsCompleted);

                        if (deleteTask.IsFaulted)
                        {
                            Debug.LogError("Lỗi khi xóa item: " + currentItemId);
                        }
                        else
                        {
                            Debug.Log("Xóa item thành công: " + currentItemId);
                        }
                        yield break; // Thoát khỏi vòng lặp sau khi xóa
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Inventory trống.");
        }
    }
}
