using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeObjectSpawn : MonoBehaviour
{
    [Header("Object Spawn")]
    public GameObject objectSpawn;

    [Header("Địa điểm Spawn Object trên ra (Default là vị trí Object hiện tại)")]
    public Transform objectSpawnPoint;

    private string realtime_object_id;

    private void Start()
    {
        realtime_object_id = RandomManager.GenerateRandomID();
    }

    public void SpawnObject()
    {
        if (objectSpawnPoint == null)
        {
            objectSpawnPoint = this.transform;
        }

        if (CountObjects.Instance.Contains(realtime_object_id)) return;

        bool isNew = CountObjects.Instance.Add(realtime_object_id);
        if (!isNew) return;

        // Instantiate không truyền parent
        GameObject obj = Instantiate(objectSpawn, objectSpawnPoint.position, Quaternion.identity);

        // Không set parent để không bị ảnh hưởng bởi object cha
        obj.GetComponent<RealtimeObjectSync>().isMainObject = true;

        RealtimeObject realtimeObject = obj.GetComponent<RealtimeObjectSync>().MySyncObject;
        while (realtimeObject == null || realtimeObject.object_id == null)
        {
            realtimeObject = obj.GetComponent<RealtimeObjectSync>().MySyncObject;
        }

        realtimeObject.realtime_object_id = realtime_object_id;

        StartCoroutine(WorldManager.AddRealtimeObject(realtimeObject));
    }
}
