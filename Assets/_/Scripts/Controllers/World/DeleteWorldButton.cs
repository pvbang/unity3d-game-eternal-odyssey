using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWorldButton : MonoBehaviour
{
    public void DeleteWorld()
    {
        NotificationManager.Instance.ShowNotification("Bạn có chắc chắn muốn xóa Thế giới này không?", NotificationType.DeleteWorld);
    }
}
