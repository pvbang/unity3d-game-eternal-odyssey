using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItems : MonoBehaviour
{

    public void Use(string itemId, int value, float valueSpeed, float duration)
    {
        Debug.Log($"Đã sử dụng item {itemId}, {value}, {valueSpeed}, {duration}");
        if (PlayerSystemController.instance == null)
        {
            Debug.LogError("playerSystemController is null trước khi gọi phương thức.");
            return; // Tránh lỗi khi đối tượng là null
        }
        switch (itemId)
        {
            case "buff_hp":
                bool healed = PlayerSystemController.instance.Heal(value);
                if (!healed)
                {
                    Debug.Log("Không sử dụng item buff_hp vì máu đã đầy.");
                    return; // Không trừ item nếu máu đã đầy
                }
                break;
            case "buff_dame":
                PlayerSystemController.instance.BuffDamage(value, duration);
                break;
            case "buff_speed":
                PlayerSystemController.instance.BuffSpeed(valueSpeed, duration);
                PlayerSystemController.instance.BuffAttackSpeed(valueSpeed, duration);
                break;
            case "buff_resistance":
                PlayerSystemController.instance.BuffResistance(value, duration);
                break;
            case "buff_crit":
                PlayerSystemController.instance.BuffCriticalChance(value, duration);
                break;
            default:
                break;
        }
    }
}
