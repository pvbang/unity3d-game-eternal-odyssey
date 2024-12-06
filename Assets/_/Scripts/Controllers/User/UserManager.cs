using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class UserManager : MonoBehaviour
{
    // thêm user mới vào database
    // cách gọi hàm: StartCoroutine(UserManager.Add(user));
    public static IEnumerator Add(User user)
    {
        string json = JsonUtility.ToJson(user);

        var task = FirebaseManager.instance.databaseReference.
            Child("users").
            Child(user.userId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm user");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm user thành công");
        }
    }


    // lấy thông tin user từ id
    // cách gọi hàm: UserManager.GetUserById("id", user => { Debug.Log(user); });
    public static void GetUserById(string userId, System.Action<User> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("users")
            .Child(userId);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin user từ id: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    User user = JsonUtility.FromJson<User>(json);
                    callback(user);
                }
                else
                {
                    callback(null);
                }
            }
        });
    }


    // cập nhật thông tin user
    // cách gọi hàm: StartCoroutine(UserManager.Update(user));
    public static IEnumerator Update(User user)
    {
        string json = JsonUtility.ToJson(user);

        var task = FirebaseManager.instance.databaseReference.
            Child("users").
            Child(user.userId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật user");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật user thành công");
        }
    }


    // xóa user
    // cách gọi hàm: StartCoroutine(UserManager.Delete(user));
    public static IEnumerator Delete(User user)
    {
        var task = FirebaseManager.instance.databaseReference.
            Child("users").
            Child(user.userId).
            RemoveValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi xóa user");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa user thành công");
        }
    }

    // lấy danh sách user
    // cách gọi hàm: UserManager.GetUsers(users => { Debug.Log(users); });
    public static void GetUsers(System.Action<List<User>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("users");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy danh sách user: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    List<User> users = new List<User>();

                    foreach (var child in snapshot.Children)
                    {
                        string json = child.GetRawJsonValue();
                        User user = JsonUtility.FromJson<User>(json);
                        users.Add(user);
                    }

                    callback(users);
                }
                else
                {
                    callback(null);
                }
            }
        });
    }


    // thêm notification cho user
    // cách gọi hàm: StartCoroutine(UserManager.AddNotification(userId, notification));
    public static IEnumerator AddNotification(string userId, Notification notification)
    {
        string json = JsonUtility.ToJson(notification);

        var task = FirebaseManager.instance.databaseReference.
            Child(ConstantManager.FIREBASE_USERS).
            Child(userId).
            Child(ConstantManager.FIREBASE_USERS_NOTIFICATIONS).
            Child(RandomManager.GenerateRandomID()).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi thêm notification cho user");
            NotificationManager.Instance.ShowNotification("Lỗi gửi lời mời gia nhập vào Thế giới", NotificationType.Warning);
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Thêm notification cho user thành công");
            NotificationManager.Instance.ShowNotification("Gửi lời mời gia nhập thế giới thành công.", NotificationType.Notification);
        }
    }

    // lấy userId từ username
    // cách gọi hàm: UserManager.GetUserIdByUsername("username", userId => { Debug.Log(userId); });

    public static void GetUserIdByUsername(string username, System.Action<string> callback)
    {
        // Tham chiếu tới Firebase database, lấy đối tượng với username cụ thể
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("users")
            .OrderByChild("username")
            .EqualTo(username);

        // Bắt đầu nhiệm vụ lấy dữ liệu từ Firebase
        var task = taskRef.GetValueAsync();

        // Tiếp tục xử lý sau khi nhận được kết quả từ Firebase
        task.ContinueWith(t =>
        {
            // Kiểm tra nếu có lỗi trong quá trình thực hiện
            if (t.IsFaulted || t.Exception != null)
            {
                Debug.LogError("Lỗi lấy userId từ username: " + t.Exception);
                callback(""); // Gọi callback với giá trị null trong trường hợp lỗi
                return;
            }

            // Kiểm tra nếu nhiệm vụ đã hoàn thành thành công
            if (t.IsCompleted)
            {
                DataSnapshot snapshot = t.Result;

                // Kiểm tra nếu dữ liệu tồn tại trong snapshot
                if (snapshot != null && snapshot.Exists && snapshot.ChildrenCount > 0)
                {
                    // Duyệt qua từng child (trường hợp chỉ có một user duy nhất với username đó)
                    foreach (var child in snapshot.Children)
                    {
                        string userId = child.Key;
                        callback(userId); // Gọi callback với userId
                        return;
                    }
                }
                else
                {
                    Debug.LogWarning("Không tìm thấy user nào với username: " + username);
                    callback(""); // Gọi callback với giá trị null nếu không có user nào
                }
            }
        });

        callback(""); // Gọi callback với giá trị null nếu không có user nào
    }


    // sửa trạng thái của notification
    // cách gọi hàm: StartCoroutine(UserManager.UpdateNotification(userId, notificationId, status));
    public static IEnumerator UpdateNotification(string userId, string notificationId, NotificationStatus status)
    {
        int statusValue = (int)status;

        var task = FirebaseManager.instance.databaseReference.
            Child("users").
            Child(userId).
            Child("notifications").
            Child(notificationId).
            Child("status").
            SetValueAsync(statusValue);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi sửa trạng thái của notification");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Sửa trạng thái của notification thành công");
        }
    }
}
