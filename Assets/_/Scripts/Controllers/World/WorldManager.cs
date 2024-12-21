using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Text;  // Để ghép chuỗi JSON

public class WorldManager : MonoBehaviour
{
    // thêm world mới vào database
    // cách gọi hàm: StartCoroutine(WorldManager.AddWorld(world));
    public static IEnumerator AddWorld(World world)
    {
        // 1. Kiểm tra và lấy dữ liệu bảng FIREBASE_WORLDS, đồng thời kiểm tra sự tồn tại của worldId
        var worldDataTask = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .GetValueAsync();

        yield return new WaitUntil(() => worldDataTask.IsCompleted);

        // Xử lý lỗi nếu không lấy được dữ liệu
        if (worldDataTask.IsFaulted)
        {
            Debug.LogError("Lỗi khi kiểm tra và lấy dữ liệu bảng FIREBASE_WORLDS.");
            yield break; // Dừng nếu có lỗi
        }

        // 2. Lấy dữ liệu của bảng và kiểm tra worldId
        var worldDataSnapshot = worldDataTask.Result;

        // Nếu bảng FIREBASE_WORLDS chưa tồn tại, khởi tạo bảng trống
        if (worldDataSnapshot.Value == null)
        {
            var initTask = FirebaseManager.instance.databaseReference
                .Child(ConstantManager.FIREBASE_WORLDS)
                .SetValueAsync("{}"); // Khởi tạo JSON trống

            yield return new WaitUntil(() => initTask.IsCompleted);

            if (initTask.IsFaulted)
            {
                NotificationManager.Instance.ShowNotification("Lỗi khi tạo bảng mới", NotificationType.Error);
                yield break; // Dừng nếu không thể khởi tạo bảng
            }
            else
            {
                Debug.Log("Bảng FIREBASE_WORLDS được tạo thành công.");
            }
        }
        else
        {
            // Nếu bảng đã tồn tại, kiểm tra sự tồn tại của worldId
            if (worldDataSnapshot.HasChild(world.worldId))
            {
                NotificationManager.Instance.ShowNotification("Mã thế giới đã tồn tại", NotificationType.Error);
                yield break; // Dừng nếu worldId đã tồn tại
            }
        }

        // Nếu qua được các bước kiểm tra, tiếp tục thực hiện các thao tác khác...

        //// Tạo Dictionary từ List<PlayerWorld> với userId làm key
        //Dictionary<string, PlayerWorld> playerWorldDict = new();
        //foreach (var player in world.playerWorld)
        //{
        //    playerWorldDict[player.userId] = player;
        //    print(player.userId);
        //}

        //// Tạo một lớp trung gian để giữ nguyên cấu trúc của World
        //WorldWithDictionary worldData = new WorldWithDictionary(world)
        //{
        //    playerWorld = playerWorldDict
        //};

        // 3. Nếu không tồn tại, tiến hành thêm world mới
        // 1. Tạo JSON thủ công cho playerWorld
        StringBuilder playerWorldJson = new StringBuilder();
        playerWorldJson.Append("{");

        for (int i = 0; i < world.playerWorld.Count; i++)
        {
            PlayerWorld player = world.playerWorld[i];

            // Chuyển PlayerWorld sang JSON
            string playerJson = JsonUtility.ToJson(player);

            // Thêm userId làm key cho JSON
            playerWorldJson.Append($"\"{player.userId}\": {playerJson}");

            // Thêm dấu phẩy nếu chưa phải phần tử cuối
            if (i < world.playerWorld.Count - 1)
            {
                playerWorldJson.Append(",");
            }
        }

        playerWorldJson.Append("}");

        // 2. Tạo JSON cho các phần còn lại của world
        string worldJson = JsonUtility.ToJson(world);

        // 3. Ghép JSON của playerWorld vào JSON của world
        string finalJson = MergeJson(worldJson, playerWorldJson.ToString());



        var addTask = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(world.worldId)
            .SetRawJsonValueAsync(finalJson);

        yield return new WaitUntil(() => addTask.IsCompleted);

        if (addTask.IsFaulted)
        {
            NotificationManager.Instance.ShowNotification("Lỗi khi tạo thế giới", NotificationType.Error);
        }
        else
        {
            NotificationManager.Instance.ShowNotification("Tạo Thế giới thành công!", NotificationType.Notification);
        }
    }

    private static string MergeJson(string worldJson, string playerWorldJson)
    {
        // Bỏ dấu ngoặc cuối của world JSON
        int lastBraceIndex = worldJson.LastIndexOf('}');
        if (lastBraceIndex == -1) return worldJson;  // Tránh lỗi nếu JSON không hợp lệ

        // Ghép JSON của playerWorld vào
        string mergedJson = worldJson.Substring(0, lastBraceIndex) +
                            ", \"playerWorld\": " + playerWorldJson + "}";

        return mergedJson;
    }

    // lấy thông tin world từ id
    // cách gọi hàm: WorldManager.GetworldById("id", world => { Debug.Log(world); });
    public static void GetWorldById(string worldId, Action<World> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy thông tin world từ id: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    World world = JsonUtility.FromJson<World>(snapshot.GetRawJsonValue());
                    callback(world);
                }
                else
                {
                    Debug.Log("Không tìm thấy world từ id");
                    callback(null);
                }
            }
        });
    }


    // lấy danh sách tất cả world
    // cách gọi hàm: WorldManager.GetAllWorlds(worlds => { Debug.Log(worlds); });
    public static void GetAllWorlds(Action<List<World>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("worlds");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy danh sách tất cả world: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                List<World> worlds = new List<World>();

                foreach (var child in snapshot.Children)
                {
                    World world = JsonUtility.FromJson<World>(child.GetRawJsonValue());
                    worlds.Add(world);
                }

                callback(worlds);
            }
        });
    }


    // lấy danh sách các world của mình
    // cách gọi hàm: WorldManager.GetMyWorlds(worlds => { Debug.Log(worlds); });
    public static void GetMyWorlds(Action<List<World>> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child("worlds");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy danh sách các world của mình: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                List<World> worlds = new List<World>();

                foreach (var child in snapshot.Children)
                {
                    World world = JsonUtility.FromJson<World>(child.GetRawJsonValue());

                    foreach (PlayerWorld playerWorld in world.playerWorld)
                    {
                        if (playerWorld.userId == FirebaseManager.instance.GetUserId())
                        {
                            worlds.Add(world);
                            break;
                        }
                    }
                }

                callback(worlds);
            }
        });
    }


    // cập nhật giá trị của world
    // cách gọi hàm: StartCoroutine(WorldManager.UpdateWorld(world));
    public static IEnumerator UpdateWorld(World world)
    {
        string json = JsonUtility.ToJson(world);

        var task = FirebaseManager.instance.databaseReference.
            Child("worlds").
            Child(world.worldId).
            SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật world");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật world thành công");
        }
    }


    // cập nhật một hoặc nhiều trường của world
    // cách gọi hàm: StartCoroutine(WorldManager.UpdateFieldsWorld("worldId", new Dictionary<string, object> { {"fieldName", fieldValue}, ... }));
    public static IEnumerator UpdateFieldsWorld(string worldId, Dictionary<string, object> fieldsToUpdate)
    {
        Debug.Log("UpdateFields");
        var task = FirebaseManager.instance.databaseReference
            .Child("worlds")
            .Child(worldId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật các trường của world");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Cập nhật các trường của world thành công");
        }
    }


    // xóa world
    // cách gọi hàm: StartCoroutine(WorldManager.DeleteWorld("id"));
    public static IEnumerator DeleteWorld(string worldId)
    {
        var task = FirebaseManager.instance.databaseReference.
            Child(ConstantManager.FIREBASE_WORLDS).
            Child(worldId).
            SetValueAsync(null);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi xóa world");
        }
        else if (task.IsCompleted)
        {
            Debug.Log("Xóa world thành công");
        }
    }

    // Thêm người chơi vào world (thêm bản thân vào playerWorld bằng worldId)
    // cách gọi hàm: StartCoroutine(WorldManager.AddPlayerToWorld(worldId));
    public static IEnumerator AddPlayerToWorld(string worldId)
    {
        // Lấy userId của người chơi hiện tại
        string userId = FirebaseManager.instance.GetUserId();

        // Kiểm tra xem worldId có tồn tại trong Firebase không
        var checkWorldTask = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .GetValueAsync();

        yield return new WaitUntil(() => checkWorldTask.IsCompleted);

        if (checkWorldTask.IsFaulted)
        {
            Debug.LogError("Lỗi khi kiểm tra worldId");
            yield break; // Dừng nếu có lỗi khi kiểm tra
        }

        if (!checkWorldTask.Result.Exists)
        {
            NotificationManager.Instance.ShowNotification("Không tìm thấy thế giới", NotificationType.Error);
            yield break; // Dừng nếu worldId không tồn tại
        }

        // Nếu worldId tồn tại, tạo đối tượng PlayerWorld mới
        PlayerWorld playerWorld = new();
        playerWorld.userId = userId;
        playerWorld.role = Role.Member;
        playerWorld.playerStatus = PlayerStatus.Online;
        playerWorld.inventorys = new List<Inventory>();
        playerWorld.playerDetails = new PlayerDetails();

        //
        Inventory inventory = new();
        inventory.itemId = "mithril";
        inventory.quantity = 3;

        Inventory inventory2 = new();
        inventory2.itemId = "mithril_hoa";
        inventory2.quantity = 5;

        Inventory inventory3 = new();
        inventory3.itemId = "mithril_thuy";
        inventory3.quantity = 2;

        playerWorld.inventorys.Add(inventory);
        playerWorld.inventorys.Add(inventory2);
        playerWorld.inventorys.Add(inventory3);

        // Chuyển đối tượng PlayerWorld thành chuỗi JSON
        string json = JsonUtility.ToJson(playerWorld);

        var taskRef = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(userId)  // Dùng userId làm key để đảm bảo mỗi user chỉ có một bản ghi
            .SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => taskRef.IsCompleted);

        if (taskRef.IsFaulted)
        {
            Debug.LogError("Lỗi thêm người chơi vào world");
        }
        else
        {
            Debug.Log("Thêm người chơi vào world thành công");
            PlayerPrefs.SetString(ConstantManager.FIREBASE_WORLDS_ID, worldId);
            NotificationManager.Instance.ShowNotification("Tham gia thế giới thành công", NotificationType.Notification);
        }
    }

    public static IEnumerator AddRealtimeObject(RealtimeObject realtimeObject)
    {
        string worldId = PlayerPrefs.GetString(ConstantManager.FIREBASE_WORLDS_ID);
        string userId = FirebaseManager.instance.GetUserId();

        // Chuyển đối tượng PlayerWorld thành chuỗi JSON
        string json = JsonUtility.ToJson(realtimeObject);

        var taskRef = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_REALTIME_OBJECTS)
            .Child(realtimeObject.realtime_object_id)  
            .SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => taskRef.IsCompleted);

        if (taskRef.IsFaulted)
        {
            Debug.LogError("Lỗi thêm realtimeObject");
        }
        else
        {
            Debug.Log("Thêm realtimeObject vào world thành công");
        }
    }

    // Cập nhật trạng thái của người chơi trong world
    // cách gọi hàm: StartCoroutine(WorldManager.UpdatePlayerStatus(worldId, userId, PlayerStatus.Online));
    public static IEnumerator UpdatePlayerStatus(string worldId, string userId, PlayerStatus playerStatus)
    {
        Dictionary<string, object> fieldsToUpdate = new Dictionary<string, object>
        {
            { "playerStatus", playerStatus }
        };

        var task = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(userId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật trạng thái của người chơi trong world");
        }
        else
        {
            Debug.Log("Cập nhật trạng thái của người chơi trong world thành công");
        }
    }

    // Cập nhật skin của người chơi trong world
    // cách gọi hàm: StartCoroutine(WorldManager.UpdatePlayerSkin(worldId, userId, skin));
    public static IEnumerator UpdatePlayerSkin(string worldId, string userId, string skin)
    {
        Dictionary<string, object> fieldsToUpdate = new Dictionary<string, object>
        {
            { "playerDetails/skin", skin }
        };

        var task = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(userId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật skin của người chơi trong world");
        }
        else
        {
            Debug.Log("Cập nhật skin của người chơi trong world thành công");
        }
    }

    // Cập nhật tên của người chơi trong world
    // cách gọi hàm: StartCoroutine(WorldManager.UpdatePlayerName(worldId, userId, name));
    public static IEnumerator UpdatePlayerName(string worldId, string userId, string name)
    {
        Dictionary<string, object> fieldsToUpdate = new Dictionary<string, object>
        {
            { "playerDetails/name", name }
        };

        var task = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(userId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật tên của người chơi trong world");
        }
        else
        {
            Debug.Log("Cập nhật tên của người chơi trong world thành công");
        }
    }


    // Lấy tên của người chơi trong world
    // cách gọi hàm: WorldManager.GetPlayerName(worldId, userId, name => { Debug.Log(name); });
    public static void GetPlayerName(string worldId, string userId, Action<string> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(userId)
            .Child("playerDetails")
            .Child("name");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy tên của người chơi trong world: " + task.Exception);
                callback("");
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    string name = snapshot.GetRawJsonValue();
                    callback(name);
                }
                else
                {
                    Debug.Log("Không tìm thấy tên của người chơi trong world");
                    callback("");
                }
            }
        });

        callback("");
    }


    // Lấy skin của người chơi trong world
    // cách gọi hàm: WorldManager.GetPlayerSkin(worldId, userId, skin => { Debug.Log(skin); });
    public static void GetPlayerSkin(string worldId, string userId, Action<string> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(userId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYER_DETAILS)
            .Child("skin");

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy skin của người chơi trong world: " + task.Exception);
                callback("");
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    string skin = snapshot.GetRawJsonValue();
                    callback(skin);
                }
                else
                {
                    Debug.Log("Không tìm thấy skin của người chơi trong world");
                    callback("");
                }
            }
        });

        callback("");
    }


    // Lấy thông tin trong world
    // cách gọi hàm: WorldManager.GetPlayerLocation(worldId, userId, value => { Debug.Log(value); });
    public static void GetPlayerLocation(string worldId, string userId, Action<PlayerWorld> callback)
    {
        var taskRef = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(userId);

        var task = taskRef.GetValueAsync();

        task.ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Lỗi lấy skin của người chơi trong world: " + task.Exception);
                callback(null);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    string value = snapshot.GetRawJsonValue();
                    PlayerWorld playerWorld = JsonUtility.FromJson<PlayerWorld>(value);
                    callback(playerWorld);
                }
                else
                {
                    callback(null);
                }
            }
        });

        callback(null);
    }


    // cập nhật thuộc tính của người chơi trong world
    // cách gọi hàm: StartCoroutine(WorldManager.UpdatePlayerDetails(worldId, userId, playerDetails));
    public static IEnumerator UpdatePlayerDetails(string worldId, string userId, PlayerDetails playerDetails)
    {
        string json = JsonUtility.ToJson(playerDetails);

        var task = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(userId)
            .Child("playerDetails")
            .SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi UpdatePlayerDetails");
        }
    }

    // cập nhật một số thuộc tính của người chơi trong world
    // cách gọi hàm: StartCoroutine(WorldManager.UpdatePlayerDetails(worldId, userId, new Dictionary<string, object> { {"fieldName", fieldValue}, ... }));
    public static IEnumerator UpdateFieldsPlayerDetails(string worldId, string userId, Dictionary<string, object> fieldsToUpdate)
    {
        var task = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(userId)
            .Child("playerDetails")
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi UpdateFieldsPlayerDetails");
        }
    }


    public static IEnumerator UpdateFieldsLocation(string worldId, string userId, Dictionary<string, object> fieldsToUpdate)
    {
        var task = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_PLAYERS)
            .Child(userId)
            .UpdateChildrenAsync(fieldsToUpdate);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi UpdateFieldsLocation");
        }
    }


    private static IEnumerator CreateObject(DatabaseReference objectRef, Dictionary<string, object> fieldsToUpdate)
    {
        Debug.Log("CreateObject");
        var createTask = objectRef.SetValueAsync(fieldsToUpdate);
        yield return new WaitUntil(() => createTask.IsCompleted);
        if (createTask.IsFaulted)
        {
            Debug.LogError("Lỗi tạo mới object!");
        }
    }

    private static IEnumerator UpdateObject(DatabaseReference objectRef, Dictionary<string, object> fieldsToUpdate)
    {
        Debug.Log("UpdateObject");
        var updateTask = objectRef.UpdateChildrenAsync(fieldsToUpdate);
        yield return new WaitUntil(() => updateTask.IsCompleted);
        if (updateTask.IsFaulted)
        {
            Debug.LogError("Lỗi cập nhật object!");
        }
    }


    public static IEnumerator UpdateOrCreateFieldsWorld(string worldId, string realtimeObjectID, Dictionary<string, object> fieldsToUpdate)
    {
        Debug.Log("UpdateOrCreateFieldsWorld");
        var objectRef = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(worldId)
            .Child(ConstantManager.FIREBASE_WORLDS_REALTIME_OBJECTS)
            .Child(realtimeObjectID);

        var checkTask = objectRef.GetValueAsync();
        yield return new WaitUntil(() => checkTask.IsCompleted);

        Debug.Log("CheckTask");

        if (checkTask.IsFaulted)
        {
            Debug.LogError($"Lỗi khi kiểm tra ObjectID: {realtimeObjectID}");
            yield break;
        }

        Debug.Log("CheckTask2");

        if (!checkTask.Result.Exists)
        {
            yield return CreateObject(objectRef, fieldsToUpdate);
        }
        else
        {
            yield return UpdateObject(objectRef, fieldsToUpdate);
        }
    }


}


public class WorldWithDictionary
{
    public string worldId;
    public string name;
    public string code;
    public List<QuestWorld> questWorld;
    public List<HouseWorld> houseWorld;
    public Dictionary<string, PlayerWorld> playerWorld;  // Dictionary để lưu với userId làm key
    public List<BuildingWorld> buildingWorld;
    public Status status;
    public string startAt;

    // Constructor để khởi tạo từ đối tượng World ban đầu
    public WorldWithDictionary(World world)
    {
        worldId = world.worldId;
        name = world.name;
        code = world.code;
        questWorld = world.questWorld;
        houseWorld = world.houseWorld;
        buildingWorld = world.buildingWorld;
        status = world.status;
        startAt = world.startAt;
    }
}