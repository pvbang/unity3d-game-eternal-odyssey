[System.Serializable]

public class User
{
    public string userId;
    public string username;
    public string email;

    public User()
    {
    }

    public User(string username, string email)
    {
        this.username = username;
        this.email = email;
    }

    public User(string userId, string username, string email)
    {
        this.userId = userId;
        this.username = username;
        this.email = email;
    }
}