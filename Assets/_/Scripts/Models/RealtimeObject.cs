using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeObject : MonoBehaviour
{
    public string realtime_object_id;
    public string object_id;
    public string type;
    public int health;
    public string animation_name;
    public Location location;
    public string status;

    public RealtimeObject()
    {
    }

    public RealtimeObject(string realtime_object_id, string object_id, string type, int health, string animation_name, Location location, string status)
    {
        this.realtime_object_id = realtime_object_id;
        this.object_id = object_id;
        this.type = type;
        this.health = health;
        this.animation_name = animation_name;
        this.location = location;
        this.status = status;
    }
}
