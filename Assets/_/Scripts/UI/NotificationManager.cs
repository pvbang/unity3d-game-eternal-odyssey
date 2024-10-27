using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Cách sử dụng: NotificationManager.instance.ShowNotification("Hello", NotificationType.Notification);
public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;

    public NotificationUI notificationUI;
    public GameObject notificationObject;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!FirebaseManager.instance.IsLogin()) return;

        DatabaseReference referenceGames = FirebaseManager.instance.databaseReference.Child(ConstantManager.FIREBASE_USERS).Child(FirebaseManager.instance.GetUserId());
        referenceGames.ValueChanged += HandleValueChangedUser;
    }


    public void ShowNotification(string content, NotificationType notificationType)
    {
        notificationUI.SetContent(content);
        notificationUI.SetNotificationType(notificationType);
        notificationObject.SetActive(true);
    }

    public void ShowNotification2(string notificationId, string content, NotificationType notificationType)
    {
        notificationUI.SetContent(content);
        notificationUI.SetNotificationType(notificationType);
        notificationUI.SetNotificationId(notificationId);
        notificationObject.SetActive(true);
    }

    public void HideNotification()
    {
        notificationObject.SetActive(false);
    }

    // lấy thông tin khi có notification mới
    void HandleValueChangedUser(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError($"Database Error: {args.DatabaseError.Message}");
            return;
        }

        if (!args.Snapshot.Exists)
        {
            Debug.LogWarning("Snapshot is empty, no data to display.");
            return;
        }

        CallNotification(args.Snapshot);
    }


    public void CallNotification(DataSnapshot data)
    {
        if (!data.Exists || !data.HasChild(ConstantManager.FIREBASE_USERS_NOTIFICATIONS)) return;

        // Thử lấy dữ liệu và ép kiểu một cách an toàn
        //List<Notification> notifications = new List<Notification>();

        foreach (DataSnapshot child in data.Child(ConstantManager.FIREBASE_USERS_NOTIFICATIONS).Children)
        {
            // Kiểm tra và chuyển đổi dữ liệu từng phần tử
            Notification notification = JsonUtility.FromJson<Notification>(child.GetRawJsonValue());
            if (notification != null)
            {
                //notifications.Add(notification); 
                if (notification.status == NotificationStatus.Read) continue;
                ShowNotification2(child.Key, notification.content, notification.notificationType);
            }
        }

        //// Duyệt qua danh sách thông báo và hiển thị
        //foreach (var notification in notifications)
        //{
        //    if (notification.status == NotificationStatus.Read) continue;
        //    ShowNotification2(notification. notification.content, notification.notificationType);
        //}
    }



    // Update database đánh dấu đã đọc notification


}
