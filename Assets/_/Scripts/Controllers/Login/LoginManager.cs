using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : BaseButton
{
    public TMP_InputField usernameText;
    public TMP_InputField passwordText;

    public GameObject UI;

    private void Awake()
    {
        // Nếu không có usernameText hay passwordText thì tìm kiếm
        if (usernameText == null)
        {
            usernameText = GameObject.Find("Username").GetComponent<TMP_InputField>();
        }
        if (passwordText == null)
        {
            passwordText = GameObject.Find("Password").GetComponent<TMP_InputField>();
        }

        // Nếu không có UI thì tìm kiếm
        if (UI == null)
        {
            UI = GameObject.Find("UI");
        }
    }

    // kiểm tra thông tin đăng nhập
    public void LoginButton()
    {
        // không được để trống
        if ((usernameText.text.Length <= 0) | (passwordText.text.Length <= 0))
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

        StartCoroutine(CheckUserLogin(usernameText.text, passwordText.text));
    }

    // đăng nhập
    IEnumerator CheckUserLogin(string username, string password)
    {
        var email = username + "@gmail.com";

        var task = FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.Log("Sai tài khoản hoặc mật khẩu");
            //Notification.instance.ShowNotifications("Sai tài khoản hoặc mật khẩu");
        }
        else if (task.IsCompleted)
        {
            AuthResult result = task.Result;
            Debug.Log("Đăng nhập thành công: " + result.User.UserId);
            UI.GetComponent<InitUI>().StartGame_();
            //Notification.instance.ShowNotifications("Đăng nhập thành công");
            //login.SetActive(false);
        }
        //WaitingController.Instance.EndWaiting();
    }

    protected override void OnClick()
    {
        LoginButton();
    }
}