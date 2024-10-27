using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // thêm world mới vào database
    // cách gọi hàm: StartCoroutine(WorldManager.AddWorld(world));
    public static IEnumerator AddWorld(World world)
    {
        print("AddWorld");
        // 1. Kiểm tra sự tồn tại của bảng (key) FIREBASE_WORLDS
        var worldListCheckTask = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .GetValueAsync();

        yield return new WaitUntil(() => worldListCheckTask.IsCompleted);

        if (worldListCheckTask.IsFaulted)
        {
            Debug.LogError("Lỗi khi kiểm tra bảng FIREBASE_WORLDS.");
            yield break; // Dừng nếu có lỗi xảy ra
        }

        // Nếu bảng chưa tồn tại, khởi tạo bảng trống
        if (worldListCheckTask.Result.Value == null)
        {
            var initTask = FirebaseManager.instance.databaseReference
                .Child(ConstantManager.FIREBASE_WORLDS)
                .SetValueAsync("{}"); // Khởi tạo JSON trống

            yield return new WaitUntil(() => initTask.IsCompleted);

            if (initTask.IsFaulted)
            {
                NotificationManager.instance.ShowNotification("Lỗi khi tạo bảng mới", NotificationType.Error);
                yield break; // Dừng nếu không thể khởi tạo bảng
            }
        }

        print("worldId: " + world.worldId);
        // 2. Tiếp tục kiểm tra sự tồn tại của worldId
        var checkTask = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(world.worldId)
            .GetValueAsync();

        yield return new WaitUntil(() => checkTask.IsCompleted);

        if (checkTask.IsFaulted)
        {
            Debug.LogError("Lỗi khi kiểm tra worldId.");
            yield break; // Dừng nếu có lỗi xảy ra
        }

        if (checkTask.Result.Exists)
        {
            NotificationManager.instance.ShowNotification("Mã thế giới đã tồn tại", NotificationType.Error);
            yield break; // Dừng nếu worldId đã tồn tại
        }

        print("Add world");
        // 3. Nếu không tồn tại, tiến hành thêm world mới
        string json = JsonUtility.ToJson(world);

        var addTask = FirebaseManager.instance.databaseReference
            .Child(ConstantManager.FIREBASE_WORLDS)
            .Child(world.worldId)
            .SetRawJsonValueAsync(json);

        yield return new WaitUntil(() => addTask.IsCompleted);

        if (addTask.IsFaulted)
        {
            NotificationManager.instance.ShowNotification("Lỗi khi tạo thế giới", NotificationType.Error);
        }
        else
        {
            NotificationManager.instance.ShowNotification("Tạo Thế giới thành công!", NotificationType.Notification);
        }
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
            NotificationManager.instance.ShowNotification("Không tìm thấy thế giới", NotificationType.Error);
            yield break; // Dừng nếu worldId không tồn tại
        }

        // Nếu worldId tồn tại, tạo đối tượng PlayerWorld mới
        PlayerWorld playerWorld = new();
        playerWorld.userId = userId;
        playerWorld.role = Role.Member;
        playerWorld.playerStatus = PlayerStatus.Online;
        playerWorld.inventorys = new List<Inventory>();
        playerWorld.playerDetails = new PlayerDetails();

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
            NotificationManager.instance.ShowNotification("Tham gia thế giới thành công", NotificationType.Notification);
        }
    }
}
