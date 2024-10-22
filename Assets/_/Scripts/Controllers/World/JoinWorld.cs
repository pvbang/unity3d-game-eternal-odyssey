using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinWorld : MonoBehaviour
{
    public TMP_InputField worldIdInputField;

    public void Join()
    {
        string worldId = worldIdInputField.text;

        if (string.IsNullOrEmpty(worldId))
        {
            NotificationManager.instance.ShowNotification("Vui lòng nhập mã thế giới", NotificationType.Error);
            return;
        }

        StartCoroutine(WorldManager.AddPlayerToWorld(worldId));
    }
}
