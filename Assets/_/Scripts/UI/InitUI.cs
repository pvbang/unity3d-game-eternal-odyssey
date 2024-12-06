using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitUI : MonoBehaviour
{
    private bool isLogin = false;

    [Header("Options")]
    public GameObject Login;
    public GameObject Register;
    public GameObject Logout;

    [Header("Input")]
    public GameObject Username;
    public GameObject Password;
    public GameObject ConfirmPassword;
    public GameObject LoginButton;
    public GameObject RegisterButton;
    public GameObject StartGameButton;

    private void Awake()
    {
        // tìm kiếm gameobject nếu chưa gán
        if (Login == null)
        {
            Login = GameObject.Find("Login");
        }
        if (Register == null)
        {
            Register = GameObject.Find("Register");
        }
        if (Logout == null)
        {
            Logout = GameObject.Find("Logout");
        }
        if (Username == null)
        {
            Username = GameObject.Find("Username");
        }
        if (Password == null)
        {
            Password = GameObject.Find("Password");
        }
        if (ConfirmPassword == null)
        {
            ConfirmPassword = GameObject.Find("ConfirmPassword");
        }
        if (LoginButton == null)
        {
            LoginButton = GameObject.Find("LoginButton");
        }
        if (RegisterButton == null)
        {
            RegisterButton = GameObject.Find("RegisterButton");
        }
        if (StartGameButton == null)
        {
            StartGameButton = GameObject.Find("StartGameButton");
        }
    }

    private void Start()
    {
        isLogin = FirebaseManager.instance.IsLogin();

        if (isLogin)
        {
            StartGame_();
        }
        else
        {
            Login_();
        }
    }

    public void Login_()
    {
        Login.SetActive(false);
        Register.SetActive(true);
        Logout.SetActive(false);

        Username.SetActive(true);
        Password.SetActive(true);
        ConfirmPassword.SetActive(false);
        LoginButton.SetActive(true);
        RegisterButton.SetActive(false);
        StartGameButton.SetActive(false);
    }

    public void Register_()
    {
        Login.SetActive(true);
        Register.SetActive(false);
        Logout.SetActive(false);

        Username.SetActive(true);
        Password.SetActive(true);
        ConfirmPassword.SetActive(true);
        LoginButton.SetActive(false);
        RegisterButton.SetActive(true);
        StartGameButton.SetActive(false);
    }

    public void StartGame_()
    {
        Login.SetActive(false);
        Register.SetActive(false);
        Logout.SetActive(true);

        Username.SetActive(false);
        Password.SetActive(false);
        ConfirmPassword.SetActive(false);
        LoginButton.SetActive(false);
        RegisterButton.SetActive(false);
        StartGameButton.SetActive(true);
    }
}
