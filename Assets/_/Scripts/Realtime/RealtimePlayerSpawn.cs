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
    private Dictionary<string, RealtimePlayerSync> spawnedPlayersRealtime = new Dictionary<string, RealtimePlayerSync>();

    private Vector3 defaultPosition = new Vector3(0f, -0.983f, 0f);
    private Vector3 defaultRotation = new Vector3(0, 0, 0);

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

    public IEnumerator DisplayLocation()
    {
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        string userId = FirebaseManager.instance.GetUserId();

        PlayerWorld playerWorld = new();
        bool checkCallback = false;

        WorldManager.GetPlayerLocation(worldId, userId, _value =>
        {
            playerWorld = _value;
            checkCallback = true;
        });

        yield return new WaitUntil(() => checkCallback);

        if (playerWorld == null)
        {
            yield break;
        }

        defaultPosition = new Vector3(playerWorld.position.x, playerWorld.position.y, playerWorld.position.z);
        defaultRotation = new Vector3(playerWorld.rotation.x, playerWorld.rotation.y, playerWorld.rotation.z);
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
                //Transform playerTransform = spawnedPlayers[playerWorld.userId];

                Vector3 position = playerWorld.position != null ? new Vector3(playerWorld.position.x, playerWorld.position.y, playerWorld.position.z) : defaultPosition;
                Vector3 rotation = playerWorld.rotation != null ? new Vector3(playerWorld.rotation.x, playerWorld.rotation.y, playerWorld.rotation.z) : defaultRotation;

                //RealtimePlayerSync playerSpawn = playerTransform.GetComponent<RealtimePlayerSync>();
                RealtimePlayerSync playerSpawn = spawnedPlayersRealtime[playerWorld.userId];

                playerSpawn.SetLocation(position, rotation);

                int stateValue = (int)playerWorld.characterState;
                CharacterState characterState = (CharacterState)stateValue;
                playerSpawn.SetCharacterState(characterState);
            }
            else
            {
                if (playerWorld.playerDetails.name == null) yield break;

                // Nếu chưa spawn, instantiate player mới và thêm vào danh sách
                Transform playerTransform = Instantiate(playerPrefab.transform);
                RealtimePlayerSync playerSpawn = playerTransform.GetComponent<RealtimePlayerSync>();

                spawnedPlayers.Add(playerWorld.userId, playerTransform); // Thêm player vào danh sách
                spawnedPlayersRealtime.Add(playerWorld.userId, playerSpawn); // Thêm player vào danh sách

                Vector3 position = playerWorld.position != null ? new Vector3(playerWorld.position.x, playerWorld.position.y, playerWorld.position.z) : defaultPosition;
                Vector3 rotation = playerWorld.rotation != null ? new Vector3(playerWorld.rotation.x, playerWorld.rotation.y, playerWorld.rotation.z) : defaultRotation;
                
                playerSpawn.SetLocation(position, rotation);

                int stateValue = (int)playerWorld.characterState;
                CharacterState characterState = (CharacterState)stateValue;
                playerSpawn.SetCharacterState(characterState);

                playerSpawn.SetSkin(playerWorld.playerDetails.skin);
            }
        }
    }
}
