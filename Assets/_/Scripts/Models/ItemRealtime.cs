using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemRealtime
{
    public string chestId;
    public Location location;
    public Dictionary<string, MaterialsUpdate> items = new Dictionary<string, MaterialsUpdate>();
    public string status;

    public ItemRealtime()
    {
    }

    public ItemRealtime(string chestId, Location location)
    {
        this.chestId = chestId;
        this.location = location;
        this.items = new Dictionary<string, MaterialsUpdate>();
        this.status = "Closed";
    }
}
