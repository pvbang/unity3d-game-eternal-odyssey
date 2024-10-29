using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NotificationUI : MonoBehaviour
{
    public TextMeshProUGUI content;
    public Animator anim;

    public Transform buttonAccept;
    public Transform buttonReject;
    public Transform buttonConfirm;
    public Transform buttonVerify;
    public Transform buttonCancel;

    public NotificationType notificationType;
    public string notificationId = "";

    public void SetContent(string content)
    {
        this.content.text = content;
    }

    public void SetNotificationId(string notificationId)
    {
        this.notificationId = notificationId;
    }

    public void SetNotificationType(NotificationType notificationType)
    {
        this.notificationType = notificationType;
    }

    //private void OnEnable()
    //{
    //    switch (notificationType)
    //    {
    //        case NotificationType.Invitation:
    //            buttonAccept.gameObject.SetActive(true);
    //            buttonReject.gameObject.SetActive(true);
    //            buttonConfirm.gameObject.SetActive(false);
    //            buttonVerify.gameObject.SetActive(false);
    //            buttonCancel.gameObject.SetActive(false);
    //            break;
    //        case NotificationType.DeleteWorld:
    //            buttonAccept.gameObject.SetActive(false);
    //            buttonReject.gameObject.SetActive(false);
    //            buttonConfirm.gameObject.SetActive(false);
    //            buttonVerify.gameObject.SetActive(true);
    //            buttonCancel.gameObject.SetActive(true);
    //            break;
    //        default:
    //            buttonAccept.gameObject.SetActive(false);
    //            buttonReject.gameObject.SetActive(false);
    //            buttonConfirm.gameObject.SetActive(true);
    //            buttonVerify.gameObject.SetActive(false);
    //            buttonCancel.gameObject.SetActive(false);
    //            break;

    //    }
    //    StartCoroutine(UserManager.UpdateNotification(FirebaseManager.instance.GetUserId(), notificationId, NotificationStatus.Read));
    //}

    public void ShowNotification(string content, NotificationType notificationType)
    {
        SetContent(content);
        SetNotificationType(notificationType);

        switch (notificationType)
        {
            case NotificationType.Invitation:
                buttonAccept.gameObject.SetActive(true);
                buttonReject.gameObject.SetActive(true);
                buttonConfirm.gameObject.SetActive(false);
                buttonVerify.gameObject.SetActive(false);
                buttonCancel.gameObject.SetActive(false);
                break;
            case NotificationType.DeleteWorld:
                buttonAccept.gameObject.SetActive(false);
                buttonReject.gameObject.SetActive(false);
                buttonConfirm.gameObject.SetActive(false);
                buttonVerify.gameObject.SetActive(true);
                buttonCancel.gameObject.SetActive(true);
                break;
            default:
                buttonAccept.gameObject.SetActive(false);
                buttonReject.gameObject.SetActive(false);
                buttonConfirm.gameObject.SetActive(true);
                buttonVerify.gameObject.SetActive(false);
                buttonCancel.gameObject.SetActive(false);
                break;
        }

        gameObject.SetActive(true);
    }


    public void ShowNotification2(string notificationId, string content, NotificationType notificationType)
    {
        ShowNotification(content, notificationType);
        StartCoroutine(UserManager.UpdateNotification(FirebaseManager.instance.GetUserId(), notificationId, NotificationStatus.Read));
    }

    //private void OnDisable()
    //{
    //    buttonAccept.gameObject.SetActive(false);
    //    buttonReject.gameObject.SetActive(false);
    //    buttonConfirm.gameObject.SetActive(false);
    //    buttonVerify.gameObject.SetActive(false);
    //    buttonCancel.gameObject.SetActive(false);

    //    content.text = "";
    //    notificationType = NotificationType.Notification;
    //    notificationId = "";
    //}

    public void CloseNotification()
    {
        // play close animation
        anim.SetTrigger("close");

        // wait for animation to finish
        StartCoroutine(CloseNotificationCoroutine());
    }

    private IEnumerator CloseNotificationCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    public string GetContent()
    {
        return content.text;
    }

    // Hàm tách mã gia nhập thế giới từ nội dung thông báo: $"Bạn nhận được lời mời gia nhập thế giới từ {PlayerPrefs.GetString(ConstantManager.FIREBASE_USERS_USERNAME)} | Mã gia nhập thế giới: {PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID)}";
    public string GetWorldIdFromContent()
    {
        print(content.text);
        string[] splitContent = content.text.Split('|');
        string worldId = splitContent[1].Trim().Split(':')[1].Trim();
        return worldId;
    }

    public void ButtonAction()
    {
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);

        switch (notificationType)
        {
            case NotificationType.Invitation:
                string _worldId = GetWorldIdFromContent();
                print(_worldId);
                if (_worldId == "") break;
                StartCoroutine(WorldManager.AddPlayerToWorld(_worldId));
                break;
            case NotificationType.DeleteWorld:
                if (worldId == "") break;
                StartCoroutine(WorldManager.DeleteWorld(worldId));
                break;
        }

        CloseNotification();
    }
}
