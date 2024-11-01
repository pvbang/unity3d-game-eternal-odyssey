using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkinPlayerController : BaseButton
{
    public TMP_InputField namePlayer;
    public SkinManager skinManager;
    public _SceneManager sceneManager;

    // Cập nhật tên người chơi
    public void UpdateNamePlayer()
    {
        string name = namePlayer.text;
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        string userId = FirebaseManager.instance.GetUserId();

        StartCoroutine(WorldManager.UpdatePlayerName(worldId, userId, name));
    }

    protected override void OnClick()
    {
        string name = namePlayer.text;

        if (name == null || name == "")
        {
            NotificationManager.instance.ShowNotification("Tên không được để trống", NotificationType.Notification);
            return;
        }

        UpdateNamePlayer();
        skinManager.UpdateSkinPlayer();

        // delay để cập nhật xong thông tin người chơi
        StartCoroutine(DelayLoadScene());

        sceneManager.LoadScene("MainScene");
    }

    IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(1f);
    }
}
