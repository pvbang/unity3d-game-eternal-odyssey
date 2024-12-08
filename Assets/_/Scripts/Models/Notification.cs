[System.Serializable]

public class Notification
{
    public string fromUserId;
    public string content;
    public NotificationStatus status;
    public NotificationType notificationType;
    public string time;

    public Notification()
    { }

    public Notification(string fromUserId, string content, NotificationStatus status, NotificationType notificationType, string time)
    {
        this.fromUserId = fromUserId;
        this.content = content;
        this.status = status;
        this.notificationType = notificationType;
        this.time = time;
    }
}