using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.Playables;

public class RegisterManager : BaseButton
{
    public TMP_InputField usernameText;
    public TMP_InputField passwordText;
    public TMP_InputField confirmPasswordText;

    public GameObject UI;

    private void Awake()
    {
        // Nếu không có usernameText, passwordText, confirmPasswordText thì tìm kiếm
        if (usernameText == null)
        {
            usernameText = GameObject.Find("Username").GetComponent<TMP_InputField>();
        }
        if (passwordText == null)
        {
            passwordText = GameObject.Find("Password").GetComponent<TMP_InputField>();
        }
        if (confirmPasswordText == null)
        {
            confirmPasswordText = GameObject.Find("ConfirmPassword").GetComponent<TMP_InputField>();
        }

        // Nếu không có UI thì tìm kiếm
        if (UI == null)
        {
            UI = GameObject.Find("UI");
        }
    }

    public void RegisterButton()
    {
        // không được để trống
        if ((usernameText.text.Length <= 0) | (passwordText.text.Length <= 0) && (confirmPasswordText.text.Length <= 0))
        {
            NotificationManager.Instance.ShowNotification("Bạn cần điền đủ thông tin", NotificationType.Warning);
            return;
        }

        // username không được chưa @gmail.com
        if (usernameText.text.Contains("@gmail.com"))
        {
            NotificationManager.Instance.ShowNotification("Tên đăng nhập không được chứa định dạng email", NotificationType.Warning);
            return;
        }

        // username phải từ 4-30 ký tự
        if ((usernameText.text.Length < 4) | (usernameText.text.Length > 30))
        {
            NotificationManager.Instance.ShowNotification("Tên đăng nhập phải từ 4-30 ký tự", NotificationType.Warning);
            return;
        }

        // mật khẩu phải từ 6-20 ký tự
        if ((passwordText.text.Length < 6) | (passwordText.text.Length > 20))
        {
            NotificationManager.Instance.ShowNotification("Mật khẩu phải từ 6-20 ký tự", NotificationType.Warning);
            return;
        }

        // mật khẩu phải giống nhau
        if (passwordText.text != confirmPasswordText.text)
        {
            NotificationManager.Instance.ShowNotification("Mật khẩu phải giống nhau", NotificationType.Warning);
            return;
        }

        StartCoroutine(CreateNewUser(usernameText.text, passwordText.text));
    }


    // tạo tài khoản mới
    IEnumerator CreateNewUser(string username, string password)
    {
        //WaitingController.Instance.StartWaiting();
        var email = username + "@gmail.com";

        var task = FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            if (task.Exception.Message.Contains("The email address is already in use by another account"))
            {
                NotificationManager.Instance.ShowNotification("Email đã tồn tại", NotificationType.Warning);
            }
            else
            {
                NotificationManager.Instance.ShowNotification("Lỗi tạo tài khoản: " + task.Exception.Message, NotificationType.Warning);
            }
        }
        else if (task.IsCompleted)
        {
            AuthResult result = task.Result;
            Dictionary<string, object> userInfo = new Dictionary<string, object>
            {
                { "userID", result.User.UserId },
                { "username", username }
            };

            FirebaseManager.instance.databaseReference.Child("users").Child(result.User.UserId).SetValueAsync(userInfo);
            UI.GetComponent<InitUI>().Login_();
            NotificationManager.Instance.ShowNotification("Tạo tài khoản thành công!", NotificationType.Notification);
        }
    }


    protected override void OnClick()
    {
        RegisterButton();
    }
}