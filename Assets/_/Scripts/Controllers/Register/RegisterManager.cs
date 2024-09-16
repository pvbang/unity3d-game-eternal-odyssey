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
            Debug.Log("Bạn cần điền đủ thông tin");
            //Notification.instance.ShowNotifications("Bạn cần điền đủ thông tin");
            return;
        }

        // username phải từ 4-30 ký tự
        if ((usernameText.text.Length < 4) | (usernameText.text.Length > 30))
        {
            Debug.Log("Tên đăng nhập phải từ 4-30 ký tự");
            //Notification.instance.ShowNotifications("Tên đăng nhập phải từ 4-30 ký tự");
            return;
        }

        // mật khẩu phải từ 6-20 ký tự
        if ((passwordText.text.Length < 6) | (passwordText.text.Length > 20))
        {
            Debug.Log("Mật khẩu phải từ 6-20 ký tự");
            //Notification.instance.ShowNotifications("Mật khẩu phải từ 6-20 ký tự");
            return;
        }

        // mật khẩu phải giống nhau
        if (passwordText.text != confirmPasswordText.text)
        {
            Debug.Log("Mật khẩu phải giống nhau");
            //Notification.instance.ShowNotifications("Mật khẩu phải giống nhau");
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
                Debug.Log("Email đã tồn tại");
                //Notification.instance.ShowNotifications("Tên tài khoản đã tồn tại");
            }
            else
            {
                Debug.Log("Lỗi tạo tài khoản");
                //Notification.instance.ShowNotifications("Lỗi tạo tài khoản");
            }
        }
        else if (task.IsCompleted)
        {
            AuthResult result = task.Result;
            Dictionary<string, object> userID = new Dictionary<string, object>
            {
                { "userID", result.User.UserId }
            };

            FirebaseManager.instance.databaseReference.Child("users").Child(result.User.UserId).SetValueAsync(userID);
            UI.GetComponent<InitUI>().Login_();
            //Notification.instance.ShowNotifications("Tạo tài khoản thành công");
            //register.SetActive(false);
        }
        //WaitingController.Instance.EndWaiting();
    }


    protected override void OnClick()
    {
        RegisterButton();
    }
}