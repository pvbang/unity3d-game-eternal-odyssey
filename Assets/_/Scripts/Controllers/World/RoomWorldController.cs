using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

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
        notification.content = $"Bạn nhận được lời mời gia nhập thế giới từ {PlayerPrefs.GetString(ConstantManager.FIREBASE_USERS_USERNAME)}";
        notification.status = NotificationStatus.Unread;
        notification.notificationType = NotificationType.Invitation;
        notification.time = TimeManager.Now();

        StartCoroutine(GetUserIdByUsername(InviteWorld.text, notification));
    }

    // hiển thị danh sách tất cả servers
    IEnumerator GetUserIdByUsername(string username, Notification notification)
    {
        string userId = "";

        UserManager.GetUserIdByUsername(username, _userId => {
            userId = _userId;
        });


        float timeout = 60f; // Thời gian chờ tối đa 30 giây
        float elapsedTime = 0f;

        while (userId == "" && elapsedTime < timeout)
        {
            // Chờ 0.1 giây mỗi lần kiểm tra
            yield return new WaitForSeconds(0.1f);

            // Cập nhật thời gian đã trôi qua
            elapsedTime += 0.1f;
        }

        if (userId == "")
        {
            Debug.Log("Không tìm thấy user");
            yield break;
        }

        AddNoti(userId, notification);
    }

    void AddNoti(string userId, Notification notification)
    {
        StartCoroutine(UserManager.AddNotification(userId, notification));
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

        StartCoroutine(UpdatePlayers(args.Snapshot));
    }

    private IEnumerator UpdatePlayers(DataSnapshot data)
    {
        if (!data.Exists) yield break;

        // Thử lấy dữ liệu và ép kiểu một cách an toàn
        List<PlayerWorld> playerWorlds = new();

        foreach (DataSnapshot child in data.Children)
        {
            // Kiểm tra và chuyển đổi dữ liệu từng phần tử
            PlayerWorld playerWorld = JsonUtility.FromJson<PlayerWorld>(child.GetRawJsonValue());
            if (playerWorld != null)
            {
                Debug.Log(playerWorld.userId);
                playerWorlds.Add(playerWorld);
            }
        }

        List<Transform> contentList = new List<Transform>();
        foreach (var playerWorld in playerWorlds)
        {
            Transform itemTransform = Instantiate(playersContent.GetItem());
            itemTransform.gameObject.SetActive(true);

            User user = new();

            UserManager.GetUserById(playerWorld.userId, _user => {
                user = _user;
            });


            float timeout = 60f; // Thời gian chờ tối đa 30 giây
            float elapsedTime = 0f;

            while (user.username == null && elapsedTime < timeout)
            {
                // Chờ 0.1 giây mỗi lần kiểm tra
                yield return new WaitForSeconds(0.1f);

                // Cập nhật thời gian đã trôi qua
                elapsedTime += 0.1f;
            }

            if (user.username == null)
            {
                Debug.Log("Không tìm thấy user");
                yield break;
            }

            itemTransform.GetComponent<UIItemPlayerWorld>().SetData(user.username);

            contentList.Add(itemTransform);
        }
        playersContent.DestroyContents();
        playersContent.AddContent(contentList);
    }
}
