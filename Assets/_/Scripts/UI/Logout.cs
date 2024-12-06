using UnityEngine;

public class Logout : BaseButton
{
    public GameObject UI;

    private void Awake()
    {
        // Nếu không có UI thì tìm kiếm
        if (UI == null)
        {
            UI = GameObject.Find("UI");
        }
    }

    protected override void OnClick()
    {
        FirebaseManager.instance.SignOut();
        Debug.Log("Đăng xuất thành công");
        UI.GetComponent<InitUI>().Login_();
    }
}
