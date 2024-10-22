using System.Collections.Generic;

[System.Serializable]

public class User
{
    public string userId;
    public string username;
    public string email;
    public List<Notification> notifications;

    public User()
    {
    }

    public User(string username, string email, List<Notification> notifications)
    {
        this.username = username;
        this.email = email;
        this.notifications = notifications;
    }

    public User(string userId, string username, string email, List<Notification> notifications)
    {
        this.userId = userId;
        this.username = username;
        this.email = email;
        this.notifications = notifications;
    }
}