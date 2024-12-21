using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectSyncManager : MonoBehaviour
{
    private DatabaseReference referenceGames;

    public static ObjectSyncManager instance;

    AnimationName currentAnimationName;
    Location currentLocation;
    Location currentRotation;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentLocation = new Location();
        currentRotation = new Location();

        if (!FirebaseManager.instance.IsLogin()) return;

        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        if (string.IsNullOrEmpty(worldId)) return;

        referenceGames = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_REALTIME_OBJECTS);

        // Đăng ký sự kiện
        referenceGames.ValueChanged += HandleValueChanged;
    }


    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError($"Database Error: {args.DatabaseError.Message}");
            return;
        }

        if (!args.Snapshot.Exists)
        {
            Debug.LogWarning("Snapshot is empty, no data to display.");
            return;
        }

        if (args.Snapshot.Exists)
            StartCoroutine(Spawn(args.Snapshot));
    }

    private IEnumerator Spawn(DataSnapshot data)
    {
        if (!data.Exists) yield break;

        // Thử lấy dữ liệu và ép kiểu một cách an toàn
        List<RealtimeObject> realtimeObjects = new();

        foreach (DataSnapshot child in data.Children)
        {
            // Kiểm tra và chuyển đổi dữ liệu từng phần tử
            RealtimeObject realtimeObject = JsonUtility.FromJson<RealtimeObject>(child.GetRawJsonValue());
            if (realtimeObject != null)
            {
                realtimeObjects.Add(realtimeObject);
            }
        }

        foreach (var realtimeObject in realtimeObjects)
        {
            bool newObject = CountObjects.Instance.Add(realtimeObject.realtime_object_id);
            if (!newObject) continue;

            GameObject objectSpawn = GameAssets.Instance.GetGameObjectFromId(realtimeObject.object_id);
            if (objectSpawn == null) continue;

            Vector3 position = new Vector3(realtimeObject.position.x, realtimeObject.position.y, realtimeObject.position.z);
            Vector3 rotation = new Vector3(realtimeObject.rotation.x, realtimeObject.rotation.y, realtimeObject.rotation.z);

            GameObject obj = Instantiate(objectSpawn, position, Quaternion.Euler(rotation));
            obj.GetComponent<RealtimeObjectSync>().MySyncObject = realtimeObject;
        }
    }

    //new Dictionary<string, object>
    //{
    //    { "experience", 10f },
    //    { "attribute/health/current", 100f }
    //}
    private void UpdateValuePlayer(string realtimeObjectID, Dictionary<string, object> changedValue)
    {
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        if (string.IsNullOrEmpty(worldId)) return;

        // Gọi hàm UpdatePlayerDetails với các thông tin cập nhật
        StartCoroutine(WorldManager.UpdateOrCreateFieldsWorld(
            worldId,
            realtimeObjectID,
            changedValue
        ));
    }


    // Hàm tạo object realtime
    public void CreateRealtimeObject(string realtimeObjectID, string object_id, RealtimeType type, Attribute attribute, AnimationName animation_name, RealtimeStatus status, Vector3 position, Vector3 rotation)
    {
        Debug.Log("CreateRealtimeObject");
        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "realtime_object_id", realtimeObjectID },
            { "object_id", object_id },
            { "type", (int)type },
            { "attribute", JsonUtility.ToJson(attribute) },
            { "animation_name", (int)animation_name },
            { "position/x", position.x },
            { "position/y", position.y },
            { "position/z", position.z },
            { "rotation/x", rotation.x },
            { "rotation/y", rotation.y },
            { "rotation/z", rotation.z },
            { "status", (int)status },
        };

        print(realtimeObjectID);
        print(JsonUtility.ToJson(attribute));
        print(object_id);
        print((int)type);
        print((int)animation_name);
        print(position);
        print(rotation);
        print((int)status);

        UpdateValuePlayer(realtimeObjectID, changedValue);
    }

    // Hàm cập nhật vị trí
    public void UpdateLocation(string realtimeObjectID, Location position, Location rotation)
    {
        if (currentLocation == position && currentRotation == rotation) return;
        currentLocation = position;
        currentRotation = rotation;

        Debug.Log("UpdateLocation");

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "position/x", position.x },
            { "position/y", position.y },
            { "position/z", position.z },
            { "rotation/x", rotation.x },
            { "rotation/y", rotation.y },
            { "rotation/z", rotation.z },
        };

        UpdateValuePlayer(realtimeObjectID, changedValue);
    }

    // Hàm cập nhật animation
    public void UpdateAnimation(string realtimeObjectID, AnimationName animationName)
    {
        if (currentAnimationName == animationName) return;
        currentAnimationName = animationName;

        Debug.Log("UpdateAnimation");

        Dictionary<string, object> changedValue = new Dictionary<string, object>
        {
            { "animation_name", (int)animationName }
        };

        UpdateValuePlayer(realtimeObjectID, changedValue);
    }
}
