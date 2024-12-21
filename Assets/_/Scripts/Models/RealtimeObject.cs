[System.Serializable]
public class RealtimeObject
{
    public string realtime_object_id;
    public string object_id;
    public RealtimeType type;
    public Attribute attribute;
    public AnimationName animation_name;
    public Location position;                   // Vị trí của người chơi
    public Location rotation;                   // Hướng nhìn của người chơi
    public RealtimeStatus status;

    public RealtimeObject()
    {
    }

    public RealtimeObject(string realtime_object_id, string object_id, RealtimeType type, Attribute attribute, AnimationName animation_name, Location position, Location rotation, RealtimeStatus status)
    {
        this.realtime_object_id = realtime_object_id;
        this.object_id = object_id;
        this.type = type;
        this.attribute = attribute;
        this.animation_name = animation_name;
        this.position = position;
        this.rotation = rotation;
        this.status = status;
    }
}
