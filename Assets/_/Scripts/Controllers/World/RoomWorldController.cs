using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomWorldController : MonoBehaviour
{
    public TMP_InputField InviteWorld;
    public Content playersContent;

    private void OnEnable()
    {
        if (!FirebaseManager.instance.IsLogin()) return;

        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        if (string.IsNullOrEmpty(worldId)) return;

        DatabaseReference referenceGames = FirebaseManager.instance.databaseReference.Child(ConstantManager.FIREBASE_WORLDS).Child(worldId).Child(ConstantManager.FIREBASE_WORLDS_PLAYERS);
        referenceGames.ValueChanged += HandleValueChangedRoomWorld;
    }

    public void AddNotification()
    {
        if (InviteWorld.text.Length <= 0)
        {
            Debug.Log("idUser is null or empty");
            return;
        }

        Notification notification = new Notification();
        notification.fromUserId = FirebaseManager.instance.GetUserId();
        notification.content = $"Bạn nhận được lời mời gia nhập thế giới từ {FirebaseManager.instance.GetUserId()}";
        notification.status = NotificationStatus.Unread;
        notification.notificationType = NotificationType.Invitation;
        notification.time = TimeManager.Now();

        StartCoroutine(UserManager.AddNotification(InviteWorld.text, notification));
    }

    void HandleValueChangedRoomWorld(object sender, ValueChangedEventArgs args)
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

        UpdatePlayers(args.Snapshot);
    }

    public void UpdatePlayers(DataSnapshot data)
    {
        if (!data.Exists) return;

        // Thử lấy dữ liệu và ép kiểu một cách an toàn
        List<PlayerWorld> playerWorlds = new();

        foreach (DataSnapshot child in data.Children)
        {
            // Kiểm tra và chuyển đổi dữ liệu từng phần tử
            PlayerWorld playerWorld = JsonUtility.FromJson<PlayerWorld>(child.GetRawJsonValue());
            if (playerWorld != null)
            {
                playerWorlds.Add(playerWorld);
            }
        }

        List<Transform> contentList = new List<Transform>();
        foreach (var playerWorld in playerWorlds)
        {
            Transform itemTransform = Instantiate(playersContent.GetItem());
            itemTransform.gameObject.SetActive(true);
            itemTransform.GetComponent<UIItemPlayerWorld>().SetData(playerWorld.userId);

            contentList.Add(itemTransform);
        }
        playersContent.DestroyContents();
        playersContent.AddContent(contentList);
    }
}
