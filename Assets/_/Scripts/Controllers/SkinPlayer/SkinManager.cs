using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public List<GameObject> skinPrefabs;

    // Hàm kiểm tra và lưu tên các GameObject đang active vào chuỗi
    public string SaveActiveSkinNames()
    {
        // Khởi tạo một danh sách tạm để lưu các tên skin active
        List<string> activeSkinNames = new List<string>();

        // Duyệt qua từng GameObject trong danh sách skinPrefabs
        foreach (GameObject skin in skinPrefabs)
        {
            // Nếu GameObject đang active thì thêm tên vào danh sách tạm
            if (skin.activeSelf)
            {
                activeSkinNames.Add(skin.name);
            }
        }

        // Ghép các tên lại thành một chuỗi, cách nhau bởi dấu phẩy
        string activeSkinNamesString = string.Join(",", activeSkinNames);

        return activeSkinNamesString;
    }

    // Hàm cập nhật skin 
    public void UpdateSkinPlayer()
    {
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        string userId = FirebaseManager.instance.GetUserId();
        string skin = SaveActiveSkinNames();

        StartCoroutine(WorldManager.UpdatePlayerSkin(worldId, userId, skin));
    }
}
