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

    public void SetContent(string content)
    {
        this.content.text = content;
    }

    public void SetNotificationType(NotificationType notificationType)
    {
        this.notificationType = notificationType;
    }

    private void OnEnable()
    {
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
    }

    private void OnDisable()
    {
        buttonAccept.gameObject.SetActive(false);
        buttonReject.gameObject.SetActive(false);
        buttonConfirm.gameObject.SetActive(false);

        content.text = "";
        notificationType = NotificationType.Notification;
    }

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

    public void ButtonAction()
    {
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);

        switch (notificationType)
        {
            case NotificationType.Invitation:
                if (worldId == "") break;
                StartCoroutine(WorldManager.AddPlayerToWorld(worldId));
                break;
            case NotificationType.DeleteWorld:
                if (worldId == "") break;
                StartCoroutine(WorldManager.DeleteWorld(worldId));
                break;
        }

        CloseNotification();
    }
}
