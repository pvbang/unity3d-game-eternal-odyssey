using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeObject : MonoBehaviour
{
    public string realtime_object_id;
    public string object_id;
    public RealtimeType type;
    public Attribute attribute;
    public CharacterState animation_name;
    public Location position;                   // Vị trí của người chơi
    public Location rotation;                   // Hướng nhìn của người chơi
    public RealtimeStatus status;

    public RealtimeObject()
    {
    }

    public RealtimeObject(string realtime_object_id, string object_id, RealtimeType type, Attribute attribute, CharacterState animation_name, Location position, Location rotation, RealtimeStatus status)
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
