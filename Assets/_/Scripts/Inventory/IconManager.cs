using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    // Danh sách các icon (Sprite) của các item
    public List<Sprite> icons = new List<Sprite>();

    // Lấy tất cả icon
    public Sprite[] GetIcons()
    {
        return icons.ToArray();
    }

    // Lấy icon theo index
    public Sprite GetIcon(int index)
    {
        if (index >= 0 && index < icons.Count)
        {
            return icons[index];
        }
        return null; // Trả về null nếu index không hợp lệ
    }

    // Lấy icon theo tên (nếu tên Sprite là tên item)
    public Sprite GetIcon(string name)
    {
        foreach (Sprite icon in icons)
        {
            if (icon.name == name)
            {
                return icon;
            }
        }
        return null; // Trả về null nếu không tìm thấy
    }
}
