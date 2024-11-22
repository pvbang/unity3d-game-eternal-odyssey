using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    public GameObject chestPrefab; // Prefab của rương

    public void SpawnChest(Vector3 itemPosition, string itemId)
    {
        GameObject chest = Instantiate(chestPrefab, itemPosition, Quaternion.identity);

        string chestId = System.Guid.NewGuid().ToString(); // Tạo ID duy nhất cho rương
        RealtimeObject chestData = new RealtimeObject
        {
            realtime_object_id = chestId,
            object_id = itemId,
            type = "chest",
            health = 0,
            animation_name = "Idle",
            location = new Location
            {
                x = itemPosition.x,
                y = itemPosition.y,
                z = itemPosition.z
            },
            status = "Closed"
        };
        FirebaseManager.instance.databaseReference.Child("realtime_object").Child(chestId).SetRawJsonValueAsync(JsonUtility.ToJson(chestData))
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Rương đã được lưu vào database!");
                }
                else
                {
                    Debug.LogError("Lỗi khi lưu rương vào database: " + task.Exception);
                }
            });
    }
}
