using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinCharacterInit : MonoBehaviour
{
    public List<GameObject> skinPrefabs;

    private void Start()
    {
        //StartCoroutine(DisplaySkin());

    }

    public IEnumerator DisplaySkin()
    {
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        string userId = FirebaseManager.instance.GetUserId();

        string skin = "";
        bool checkCallback = false;

        WorldManager.GetPlayerSkin(worldId, userId, _skin =>
        {
            skin = _skin;
            checkCallback = true;
        });

        yield return new WaitUntil(() => checkCallback);

        // Chia chuỗi skin thành danh sách tên
        string[] skinNames = skin.Split(',');

        // Duyệt qua tất cả các skinPrefabs
        foreach (GameObject skinPrefab in skinPrefabs)
        {
            // Nếu tên của skinPrefab có trong danh sách skinNames thì kích hoạt, ngược lại thì tắt
            string skinName = skinPrefab.name;
            if (skinName == "pelvis") skinName = "Pelvis1";
            bool shouldBeActive = skinNames.Contains(skinName);
            skinPrefab.SetActive(shouldBeActive);
        }
    }
}
