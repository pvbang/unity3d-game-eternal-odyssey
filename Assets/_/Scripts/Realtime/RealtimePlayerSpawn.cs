using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class RealtimePlayerSpawn : MonoBehaviour
{
    public GameObject playerPrefab;
    private DatabaseReference referenceGames;

    // Danh sách các player đã được instantiate
    private Dictionary<string, Transform> spawnedPlayers = new Dictionary<string, Transform>();

    private Vector3 defaultPosition = new Vector3(143.18f, 11.43f, 136.3f);
    
    private void Start()
    {
        if (!FirebaseManager.instance.IsLogin()) return;

        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        if (string.IsNullOrEmpty(worldId)) return;

        referenceGames = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS);

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

        StartCoroutine(SpawnPlayers(args.Snapshot));
    }

    private IEnumerator SpawnPlayers(DataSnapshot data)
    {
        if (!data.Exists) yield break;

        // Thử lấy dữ liệu và ép kiểu một cách an toàn
        List<PlayerWorld> playerWorlds = new();

        foreach (DataSnapshot child in data.Children)
        {
            // Kiểm tra và chuyển đổi dữ liệu từng phần tử
            PlayerWorld playerWorld = JsonUtility.FromJson<PlayerWorld>(child.GetRawJsonValue());
            if (playerWorld != null)
            {
                Debug.Log(playerWorld.userId);
                playerWorlds.Add(playerWorld);
            }
        }

        foreach (var playerWorld in playerWorlds)
        {
            if (playerWorld.userId == FirebaseManager.instance.GetUserId()) continue;

            // Kiểm tra nếu player đã tồn tại trong danh sách spawnedPlayers
            if (spawnedPlayers.ContainsKey(playerWorld.userId))
            {
                // Cập nhật vị trí của player đã tồn tại
                Transform playerTransform = spawnedPlayers[playerWorld.userId];

                // Kiểm tra nếu position không có trong dữ liệu
                Vector3 position = playerWorld.position != null ? new Vector3(playerWorld.position.x, playerWorld.position.y, playerWorld.position.z) : defaultPosition;
                playerTransform.position = position;

                // Kiểm tra nếu rotation không có trong dữ liệu
                Quaternion rotation = playerWorld.rotation != null ? Quaternion.Euler(playerWorld.rotation.x, playerWorld.rotation.y, playerWorld.rotation.z) : Quaternion.identity;
                playerTransform.rotation = rotation;

                RealtimePlayerSync playerSpawn = playerTransform.GetComponent<RealtimePlayerSync>();
                int stateValue = (int)playerWorld.characterState;
                CharacterState characterState = CharacterState.Idle;
                if (Enum.IsDefined(typeof(CharacterState), stateValue))
                {
                    characterState = (CharacterState)stateValue;
                }
                playerSpawn.SetCharacterState(characterState);
            }
            else
            {
                // Nếu chưa spawn, instantiate player mới và thêm vào danh sách
                Transform playerTransform = Instantiate(playerPrefab.transform);

                // Kiểm tra nếu position không có trong dữ liệu
                Vector3 position = playerWorld.position != null ? new Vector3(playerWorld.position.x, playerWorld.position.y, playerWorld.position.z) : defaultPosition;
                playerTransform.position = position;

                // Kiểm tra nếu rotation không có trong dữ liệu
                Quaternion rotation = playerWorld.rotation != null ? Quaternion.Euler(playerWorld.rotation.x, playerWorld.rotation.y, playerWorld.rotation.z) : Quaternion.identity;
                playerTransform.rotation = rotation;

                RealtimePlayerSync playerSpawn = playerTransform.GetComponent<RealtimePlayerSync>();
                int stateValue = (int)playerWorld.characterState;
                CharacterState characterState = CharacterState.Idle;
                if (Enum.IsDefined(typeof(CharacterState), stateValue))
                {
                    characterState = (CharacterState)stateValue;
                }
                playerSpawn.SetCharacterState(characterState);

                playerSpawn.SetSkin(playerWorld.playerDetails.skin);

                spawnedPlayers.Add(playerWorld.userId, playerTransform); // Thêm player vào danh sách
            }
        }
    }
}
