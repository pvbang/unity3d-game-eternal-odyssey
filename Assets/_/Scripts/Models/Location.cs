using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Location : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public Location() { }

    public Location(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}
