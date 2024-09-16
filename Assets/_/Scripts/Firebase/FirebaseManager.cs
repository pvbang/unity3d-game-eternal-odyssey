using UnityEngine;
using Firebase.Database;
using Firebase.Auth;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager instance;

    public DatabaseReference databaseReference;

    private void Awake()
    {
        FirebaseManager.instance = this;
        this.databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // kiểm tra xem đã đăng nhập chưa
    public bool IsLogin()
    {
        try {
            return FirebaseAuth.DefaultInstance.CurrentUser != null;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }

    // lấy id của user
    public string GetUserId()
    {
        return FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    }

    // đăng xuất
    public void SignOut()
    {
        FirebaseAuth.DefaultInstance.SignOut();
    }
}
