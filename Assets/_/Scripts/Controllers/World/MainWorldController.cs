using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Globalization;
using Firebase.Database;

public class MainWorldController : MonoBehaviour
{
    public UIMainWorld UIMainWorld;

    public Content contentMyWorlds;
    public Content contentAllWorlds;

    List<World> allWorlds = new List<World>();
    List<World> myWorlds = new List<World>();

    private void Awake()
    {
        if (UIMainWorld != null) return;
        UIMainWorld = GetComponent<UIMainWorld>();
    }

    private void Start()
    {
        if (!FirebaseManager.instance.IsLogin()) return;

        DatabaseReference referenceGames = FirebaseManager.instance.databaseReference.Child(ConstantManager.FIREBASE_WORLDS);
        referenceGames.ValueChanged += HandleValueChangedWorlds;
    }

    void HandleValueChangedWorlds(object sender, ValueChangedEventArgs args)
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

        UpdateMyWorlds(args.Snapshot);
        UpdateAllWorlds(args.Snapshot);
    }

    public void UpdateMyWorlds(DataSnapshot data)
    {
        if (!data.Exists) return;

        List<World> worlds = new List<World>();

        foreach (DataSnapshot child in data.Children)
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

        // nếu worlds không thay đổi thì không cần cập nhật
        if (worlds.Count == myWorlds.Count && worlds.All(myWorlds.Contains) && myWorlds.All(worlds.Contains)) return;
        ShowListMyWorlds(worlds);
    }

    public void UpdateAllWorlds(DataSnapshot data)
    {
        if (!data.Exists) return;

        List<World> worlds = new List<World>();

        foreach (DataSnapshot child in data.Children)
        {
            World world = JsonUtility.FromJson<World>(child.GetRawJsonValue());
            worlds.Add(world);
        }

        // nếu worlds không thay đổi thì không cần cập nhật
        if (worlds.Count == allWorlds.Count && worlds.All(allWorlds.Contains) && allWorlds.All(worlds.Contains)) return;
        ShowListAllWorlds(worlds);
    }

    public void CreateWorld()
    {
        if (string.IsNullOrEmpty(UIMainWorld.GetNameWorld()))
        {
            NotificationManager.instance.ShowNotification("Vui lòng nhập Tên thế giới!", NotificationType.Notification);
            return;
        }
        if (string.IsNullOrEmpty(UIMainWorld.GetCodeWorld()))
        {
            NotificationManager.instance.ShowNotification("Vui lòng nhập Mã thế giới!", NotificationType.Notification);
            return;
        }

        World world = new();
        world.worldId = UIMainWorld.GetCodeWorld();
        world.name = UIMainWorld.GetNameWorld();
        world.code = UIMainWorld.GetCodeWorld();
        world.questWorld = new List<QuestWorld>();
        world.houseWorld = new List<HouseWorld>();
        world.playerWorld = new List<PlayerWorld>();
        world.buildingWorld = new List<BuildingWorld>();
        world.status = Status.NotStarted;
        world.startAt = TimeManager.Now();

        PlayerWorld playerWorld = new();
        playerWorld.userId = FirebaseManager.instance.GetUserId();
        playerWorld.role = Role.RoomOwner;
        playerWorld.inventorys = new List<Inventory>();

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

        world.playerWorld.Add(playerWorld);

        StartCoroutine(WorldManager.AddWorld(world));
    }


    /// <summary>
    /// Hiển thị danh sách tất cả worlds
    /// </summary>
    public void ShowListAllWorlds(List<World> worlds)
    {
        contentAllWorlds.DestroyContents();

        // Chuyển đổi thời gian từ chuỗi sang DateTime và tính thời gian hoàn thành
        var rankedWorlds = worlds
            .Select(world =>
            {
                // Kiểm tra và chuyển đổi startAt, endAt thành DateTime
                DateTime startAt = ParseDateTimeSafe(world.startAt);
                DateTime? endAt = string.IsNullOrEmpty(world.endAt)
                    ? (DateTime?)null
                    : ParseDateTimeSafe(world.endAt);

                // Tính thời gian hoàn thành nếu có endAt, nếu không trả về null
                TimeSpan? completionTime = endAt.HasValue ? endAt.Value - startAt : null;

                return (world, completionTime);
            })
            // Sắp xếp: Thời gian hoàn thành ngắn nhất trước, nếu chưa hoàn thành thì theo startAt sớm nhất
            .OrderBy(entry => entry.completionTime ?? TimeSpan.MaxValue)
            .ThenBy(entry => ParseDateTimeSafe(entry.world.startAt))
            .ToList();

        // Khởi tạo danh sách contentList để hiển thị
        List<Transform> contentList = new List<Transform>();

        // Duyệt qua danh sách đã xếp hạng và tạo các item với thứ hạng
        for (int i = 0; i < rankedWorlds.Count; i++)
        {
            var (world, _) = rankedWorlds[i];
            int rank = i + 1; // Thứ hạng bắt đầu từ 1

            // Khởi tạo item và gán dữ liệu
            Transform itemTransform = Instantiate(contentAllWorlds.GetItem());
            itemTransform.gameObject.SetActive(true);

            // Gán dữ liệu cho UI item
            itemTransform.GetComponent<UIItemAllWorld>().SetData(
                rank.ToString(),
                world.name,
                world.status.ToString(),
                world.startAt,
                world.endAt
            );

            // Lưu worldId vào PlayerPrefManager
            itemTransform.GetComponent<PlayerPrefManager>().SetData(ConstantManager.FIREBASE_WORLDS_ID, world.worldId);

            // Thêm item vào contentList
            contentList.Add(itemTransform);
        }

        // Thêm tất cả các item đã tạo vào contentAllWorlds
        contentAllWorlds.AddContent(contentList);
    }

    // Hàm chuyển đổi chuỗi ngày giờ an toàn, trả về DateTime.MinValue nếu không hợp lệ
    private DateTime ParseDateTimeSafe(string dateTime)
    {
        // Cố gắng parse ngày giờ với định dạng cụ thể, nếu không hợp lệ trả về DateTime.MinValue
        if (DateTime.TryParseExact(
                dateTime,
                "yyyy-MM-dd.HH:mm:ss:fff",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime result))
        {
            return result;
        }
        else
        {
            return DateTime.MinValue; // Giá trị mặc định nếu parse thất bại
        }


    }
    // Hàm kiểm tra xem chuỗi ngày giờ có hợp lệ không
    private bool IsValidDateTime(string dateTime)
    {
        return DateTime.TryParseExact(
            dateTime,
            "yyyy-MM-dd.HH:mm:ss:fff",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);
    }

    /// <summary>
    /// Hiển thị danh sách các worlds của mình
    /// </summary>
    //public void ShowListMyWorlds()
    //{
    //    StartCoroutine(ShowListMyWorlds_());
    //}

    public void ShowListMyWorlds(List<World> worlds)
    {
        contentMyWorlds.DestroyContents();

        // Sắp xếp danh sách thế giới theo ngày bắt đầu (muộn nhất ở trên)
        var sortedWorlds = worlds
            .Where(world => IsValidDateTime(world.startAt)) // Lọc ra những world có thời gian hợp lệ
            .OrderByDescending(world =>
                ParseDateTimeSafe(world.startAt)) // Parse ngày giờ an toàn
            .ToList();

        // Tạo danh sách các item UI để hiển thị
        List<Transform> contentList = new List<Transform>();

        foreach (World world in sortedWorlds)
        {
            Transform itemTransform = Instantiate(contentMyWorlds.GetItem());
            itemTransform.gameObject.SetActive(true);

            // Gán dữ liệu vào UI item
            itemTransform.GetComponent<UIItemWorld>().SetData(
                world.name,
                world.status.ToString(),
                world.code,
                world.startAt,
                world.endAt
            );

            // Lưu worldId vào PlayerPrefManager
            itemTransform.GetComponent<PlayerPrefManager>().SetData(ConstantManager.FIREBASE_WORLDS_ID, world.worldId);

            contentList.Add(itemTransform);
        }

        // Thêm tất cả các item đã tạo vào contentMyWorlds
        contentMyWorlds.AddContent(contentList);
    }
}
