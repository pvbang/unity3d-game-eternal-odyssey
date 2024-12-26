using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealtimeObjectSync : MonoBehaviour
{
    [Header("True => Sync object này lên Firebase\nFalse => Cập nhật trạng thái từ Firebase về")]
    public bool isMainObject = false;

    public RealtimeObject MySyncObject;

    public EnemyAtribute attribute;

    private DatabaseReference referenceGames;

    public Animator animator;

    // Các biến lưu trạng thái cho nội suy
    private Vector3 currentTargetPosition;
    private Quaternion currentTargetRotation;

    private float positionSmoothTime = 0.25f; // Thời gian để chuyển mượt
    private float rotationSmoothTime = 0.25f; // Thời gian để quay mượt

    public float syncInterval = 0.25f; // Tần suất đồng bộ với server (200ms)
    private float syncTimer = 0f;

    private Vector3 defaultPosition = new Vector3(0f, 0f, 0f);
    private Vector3 defaultRotation = new Vector3(0, 0, 0);

    private AnimationName currentAnimationName = AnimationName.idle;

    private void Awake()
    {
        MySyncObject = new RealtimeObject();

        if (MySyncObject.object_id == null)
            MySyncObject.object_id = GetComponent<ID>()?.GetID() ?? "UnknownID";

        if (attribute == null)
            attribute = GetComponent<EnemyAtribute>();

        if (attribute != null)
        {
            MySyncObject.attribute = attribute.atribute;
        }
        else
        {
            Debug.LogWarning($"EnemyAtribute is missing on {gameObject.name}.");
            EnemyAtribute enemyAtribute = new EnemyAtribute();
            MySyncObject.attribute = enemyAtribute.atribute; // Tạo attribute mặc định nếu cần
        }

        MySyncObject.animation_name = currentAnimationName;
        MySyncObject.status = RealtimeStatus.Active;

        if (MySyncObject.position == null)
            MySyncObject.position = new Location();
        if (MySyncObject.rotation == null)
            MySyncObject.rotation = new Location();

        currentTargetPosition = new Vector3(MySyncObject.position.x, MySyncObject.position.y, MySyncObject.position.z);
        currentTargetRotation = Quaternion.Euler(MySyncObject.rotation.x, MySyncObject.rotation.y, MySyncObject.rotation.z);

        defaultPosition = currentTargetPosition;
        defaultRotation = currentTargetRotation.eulerAngles;
    }


    private void Start()
    {
        currentTargetPosition = transform.localPosition;
        currentTargetRotation = transform.localRotation;

        if (!FirebaseManager.instance.IsLogin()) return;

        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        if (string.IsNullOrEmpty(worldId)) return;

        referenceGames = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_REALTIME_OBJECTS)
            .Child(MySyncObject.realtime_object_id);

        // Đăng ký sự kiện
        referenceGames.ValueChanged += HandleValueChangedWorld;
    }

    private void OnDisable()
    {
        // Hủy đăng ký sự kiện để tránh việc sự kiện còn tồn tại khi chuyển scene
        if (referenceGames != null)
        {
            referenceGames.ValueChanged -= HandleValueChangedWorld;
        }
    }

    void HandleValueChangedWorld(object sender, ValueChangedEventArgs args)
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

        StartCoroutine(Spawn(args.Snapshot));
    }

    private IEnumerator Spawn(DataSnapshot data)
    {
        if (!data.Exists) yield break;

        Debug.Log($"[Spawn] RealtimeObjectSync: {data.GetRawJsonValue()}");
        RealtimeObject realtimeObject = JsonUtility.FromJson<RealtimeObject>(data.GetRawJsonValue());
        SetLocation(new Vector3(realtimeObject.position.x, realtimeObject.position.y, realtimeObject.position.z), new Vector3(realtimeObject.rotation.x, realtimeObject.rotation.y, realtimeObject.rotation.z));
        SetAnimation(realtimeObject.animation_name);
    }



    private void Update()
    {
        if (MySyncObject.realtime_object_id == null) return;

        syncTimer += Time.deltaTime;
        if (syncTimer < syncInterval) return;
        syncTimer = 0f;

        Debug.Log($"[Update] RealtimeObjectSync: {MySyncObject.realtime_object_id}");

        if (isMainObject)
        {
            Location position = new Location
            {
                x = transform.localPosition.x,
                y = transform.localPosition.y,
                z = transform.localPosition.z
            };

            Location rotation = new Location
            {
                x = transform.localRotation.eulerAngles.x,
                y = transform.localRotation.eulerAngles.y,
                z = transform.localRotation.eulerAngles.z
            };

            if (MySyncObject.position != position || MySyncObject.rotation != rotation)
            {
                Debug.Log($"[Update] RealtimeObjectSync 1: {MySyncObject.realtime_object_id}");
                MySyncObject.position = position;
                MySyncObject.rotation = rotation;
                ObjectSyncManager.instance.UpdateLocation(MySyncObject.realtime_object_id, position, rotation);
            }
            
            if (MySyncObject.animation_name != currentAnimationName) ObjectSyncManager.instance.UpdateAnimation(MySyncObject.realtime_object_id, currentAnimationName);
        }
        else
        {
            Debug.Log($"[Update] RealtimeObjectSync 2: {MySyncObject.realtime_object_id}");
            // Nội suy vị trí
            transform.localPosition = Vector3.Lerp(transform.localPosition, currentTargetPosition, Time.deltaTime / positionSmoothTime);

            // Nội suy góc quay
            transform.localRotation = Quaternion.Lerp(transform.localRotation, currentTargetRotation, Time.deltaTime / rotationSmoothTime);
        }
    }

    // Hàm set vị trí cho player với nội suy
    public void SetLocation(Vector3 position, Vector3 rotation)
    {
        if (position == defaultPosition && rotation == defaultRotation)
        {
            return;
        }

        defaultPosition = position;
        defaultRotation = rotation;

        currentTargetPosition = position;
        currentTargetRotation = Quaternion.Euler(rotation);

        // Nội suy vị trí
        transform.localPosition = Vector3.Lerp(transform.localPosition, currentTargetPosition, Time.deltaTime / positionSmoothTime);

        // Nội suy góc quay
        transform.localRotation = Quaternion.Lerp(transform.localRotation, currentTargetRotation, Time.deltaTime / rotationSmoothTime);
    }


    /////////////////////////////////////////////////////////////////////////////////////////
    // ANIMATION

    // Hàm quản lý trạng thái Animator
    public void SetAnimation(AnimationName animationName)
    {
        if (animator == null) return;
        if (animationName == currentAnimationName) return;

        currentAnimationName = animationName;
        MySyncObject.animation_name = animationName;

        if (animationName != AnimationName.idle) animator.SetBool("idle", false);

        animator.SetTrigger(animationName.ToString());
    }

    public void UpdateAnimation(AnimationName animationName)
    {
        MySyncObject.animation_name = animationName;
    }
}
